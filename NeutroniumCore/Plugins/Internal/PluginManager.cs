using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using HarmonyLib;
using Neutronium.Core.Logging.Api;
using Neutronium.Core.Meta;
using Neutronium.Core.Paths.Api;
using Neutronium.Core.Plugins.Api;

namespace Neutronium.Core.Plugins.Internal
{
	internal static class PluginManager
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.PluginManager");

		private static MetadataAssemblyResolver s_assemblyResolver;

		private static readonly Dictionary<string, PluginWrapper> s_plugins = new Dictionary<string, PluginWrapper>();

		internal static void Initialize()
		{
			s_log.Info("Initializing...");

			List<string> dependencyAssemblyFolders = new List<string>();
			dependencyAssemblyFolders.Add(RuntimeEnvironment.GetRuntimeDirectory());
			dependencyAssemblyFolders.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
			dependencyAssemblyFolders.Add(FilePaths.GameFolder.OxygenNotIncluded_Data.Managed);

			List<string> assemblyPaths = new List<string>();
			foreach (var dependencyFolder in dependencyAssemblyFolders)
			{
				foreach (var dllFile in Directory.GetFiles(dependencyFolder, "*.dll", SearchOption.TopDirectoryOnly))
				{
					assemblyPaths.Add(dllFile);
				}
			}
			
			s_assemblyResolver = new PathAssemblyResolver(assemblyPaths);

			LoadAllPlugins();
		}

		private static void LoadAllPlugins()
		{
			s_log.Info("Loading plugins...");
			
			List<PluginFile> incompatiblePlugins = new List<PluginFile>();
			List<PluginFile> badPlugins = new List<PluginFile>();
			
			foreach (var pluginFile in ScanForPlugins())
			{
				IPlugin plugin = LoadPlugin(pluginFile);
				if (plugin == null) continue;

				string pluginId = null;
				try
				{
					string uniqueId = plugin.UniqueID;
					pluginId = $"{pluginFile.ModStaticId}:{uniqueId}";
					
					if (plugin.MinGameVersion.HasValue)
					{
						if (plugin.MinGameVersion.Value > GameVersion.RunningGameVersion)
						{
							s_log.Warn($"Plugin ignored due to min game version requirement.\nPlugin: {pluginId}\nMin Game Ver: {plugin.MinGameVersion.Value}");
							incompatiblePlugins.Add(pluginFile);
							continue;
						}
					}

					if (plugin.MaxGameVersion.HasValue)
					{
						if (plugin.MaxGameVersion.Value < GameVersion.RunningGameVersion)
						{
							s_log.Warn($"Plugin ignored due to max game version requirement.\nPlugin: {uniqueId}\nMin Game Ver: {plugin.MaxGameVersion.Value}");
							incompatiblePlugins.Add(pluginFile);
							continue;
						}
					}
				}
				catch (Exception ex)
				{
					s_log.Error($"Error while checking plugin requirements: {pluginFile.FilePath}", ex);
					badPlugins.Add(pluginFile);
					continue;
				}
				
				try
				{
					ILogger pluginLogger = LoggerFactory.GetLogger(pluginId);
					plugin.ProvideLogger(pluginLogger);
				}
				catch (Exception ex)
				{
					s_log.Error($"Error while providing logger to plugin: {pluginId}", ex);
					badPlugins.Add(pluginFile);
					continue;
				}

				s_plugins.Add(pluginId, new PluginWrapper(plugin, pluginFile.FilePath, pluginFile.ModStaticId));
				s_log.Info($"Plugin loaded: {pluginId}");
			}
			
			if (badPlugins.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine($"{badPlugins.Count} plugins failed to load correctly:");
				for (int i=0; i<badPlugins.Count; i++)
				{
					sb.AppendLine(badPlugins[i].FilePath);
				}
				s_log.Warn(sb.ToString());
			}
			
			if (incompatiblePlugins.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine($"{incompatiblePlugins.Count} plugins were not compatible with this game version:");
				for (int i=0; i<incompatiblePlugins.Count; i++)
				{
					sb.AppendLine(incompatiblePlugins[i].FilePath);
				}
				s_log.Warn(sb.ToString());
			}
			
			if (s_plugins.Count == 0)
			{
				s_log.Info("No valid plugins found.");
				return;
			}
			
			s_log.Info("Loading assets for plugins...");
			int numAssetLoaders = 0;
			int numAssetErrors = 0;
			foreach (var kvp in s_plugins)
			{
				if (!kvp.Value.IsAssetsPlugin) continue;
				var id = kvp.Value.Id;
				
				try
				{
					kvp.Value.LoadAssets();
					s_log.Info($"Loaded assets for plugin: {id}");
					numAssetLoaders++;
				}
				catch (Exception ex)
				{
					s_log.Error($"Failed to load assets for plugin: {id}");
					numAssetErrors++;
				}
			}
			s_log.Info($"{numAssetLoaders} Asset Loaders Succeeded, {numAssetErrors} Asset Loaders Failed.");

			s_log.Info("Applying patches for plugins...");
			int numPatchers = 0;
			int numPatcherErrors = 0;
			foreach (var kvp in s_plugins)
			{
				if (!kvp.Value.IsPatcher) continue;
				var id = kvp.Value.Id;

				try
				{
					kvp.Value.ApplyPatches();
					s_log.Info($"Applied patches for plugin: {id}");
					numPatchers++;
				}
				catch (Exception ex)
				{
					s_log.Error($"Failed to apply patches for plugin: {id}", ex);
					numPatcherErrors++;
				}
			}
			s_log.Info($"{numPatchers} Patchers Succeeded, {numPatcherErrors} Patchers Failed.");
		}

		/// <summary>
		/// Enumerates plugin DLLs found in all mod platform folders.
		/// </summary>
		private static IEnumerable<PluginFile> ScanForPlugins()
		{
			foreach (var pluginFile in ScanForPlugins(FilePaths.ActiveDataFolder.Mods.Dev))
			{
				yield return pluginFile;
			}

			foreach (var pluginFile in ScanForPlugins(FilePaths.ActiveDataFolder.Mods.Local))
			{
				yield return pluginFile;
			}

			foreach (var pluginFile in ScanForPlugins(FilePaths.ActiveDataFolder.Mods.Steam))
			{
				yield return pluginFile;
			}
		}

		/// <summary>
		/// Enumerates plugin DLLs found in a specific mod platform folder.
		/// </summary>
		private static IEnumerable<PluginFile> ScanForPlugins(string modPlatformFolder)
		{
			if (string.IsNullOrWhiteSpace(modPlatformFolder)) throw new ArgumentNullException(nameof(modPlatformFolder));
			if (!Directory.Exists(modPlatformFolder)) yield break;

			string platform = Path.GetFileName(modPlatformFolder);
			
			foreach (var modFolder in Directory.GetDirectories(modPlatformFolder, "*", SearchOption.TopDirectoryOnly))
			{
				foreach (var pluginFile in ScanModForPlugins(modFolder, platform))
				{
					yield return pluginFile;
				}
			}
		}

		/// <summary>
		/// Enumerates plugin DLLs found in a specific mod folder.
		/// </summary>
		private static IEnumerable<PluginFile> ScanModForPlugins(string modFolder, string platform)
		{
			if (string.IsNullOrWhiteSpace(modFolder)) throw new ArgumentNullException(nameof(modFolder));
			if (!Directory.Exists(modFolder)) yield break;
			
			string neutroniumSubfolder = Path.Combine(modFolder, "Neutronium");
			if (!Directory.Exists(neutroniumSubfolder)) yield break;

			string pluginsFolder = Path.Combine(neutroniumSubfolder, "Plugins");
			if (!Directory.Exists(pluginsFolder)) yield break;

			string staticId;
			string modYamlFile = Path.Combine(modFolder, "mod.yaml");
			if (File.Exists(modYamlFile))
			{
				// TODO: Parse out staticID from mod.yaml
				staticId = null;
			}
			else
			{
				string folderName = Path.GetFileName(modFolder);
				staticId = $"{folderName}.{platform}";
			}

			foreach (var pluginFile in Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.TopDirectoryOnly))
			{
				yield return new PluginFile(platform, staticId, pluginFile);
			}
		}

		private static IPlugin LoadPlugin(PluginFile pluginFile)
		{
			if (!File.Exists(pluginFile.FilePath))
			{
				s_log.Error($"Plugin DLL is missing: {pluginFile.FilePath}");
				return null;
			}

			if (!HasIPluginClass(pluginFile.FilePath))
			{
				s_log.Error($"Plugin DLL is missing IPlugin interface: {pluginFile.FilePath}");
				return null;
			}

			return ActuallyLoadPlugin(pluginFile.FilePath);
		}

		private static bool HasIPluginClass(string assemblyPath)
		{
			try
			{
				using MetadataLoadContext ctx = new MetadataLoadContext(s_assemblyResolver);
				Type pluginInterface = null;
				var assembly = ctx.LoadFromAssemblyPath(assemblyPath);
				foreach (var type in assembly.GetExportedTypes())
				{
					var tempInterface = type.GetInterface(nameof(IPlugin));
					if (tempInterface != null)
					{
						if (pluginInterface != null)
						{
							s_log.Error($"Plugin DLL has multiple IPlugin classes: {assemblyPath}");
							return false;
						}
						pluginInterface = tempInterface;
					}
				}

				if (pluginInterface == null)
				{
					s_log.Error($"Plugin DLL has no IPlugin class: {assemblyPath}");
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				s_log.Error("Error while inspecting assembly.", ex);
				return false;
			}
		}

		private static IPlugin ActuallyLoadPlugin(string assemblyPath)
		{
			try
			{
				var assembly = Assembly.LoadFile(assemblyPath);
				Type pluginInterface = null;
				foreach (var type in assembly.GetExportedTypes())
				{
					pluginInterface = type.GetInterface(nameof(IPlugin));
					if (pluginInterface != null) break;
				}

				if (pluginInterface == null) throw new Exception("Could not find IPlugin type.");

				IPlugin plugin = Activator.CreateInstance(pluginInterface) as IPlugin;
				return plugin;
			}
			catch (Exception ex)
			{
				s_log.Error($"Failed to load plugin: {assemblyPath}");
				return null;
			}
		}

		internal static void BeforeDbInitialized()
		{
			s_log.Debug("Running BeforeDbInitialized for plugins...");
			if (s_plugins.Count == 0) return;
			foreach (var kvp in s_plugins)
			{
				var plugin = kvp.Value;
				if (!plugin.IsDbInitializer) continue;
				try
				{
					plugin.BeforeDbInitialized();
				}
				catch (Exception ex)
				{
					s_log.Error($"Error in plugin BeforeDbInitialized: {plugin.Id}", ex);
				}
			}
		}

		internal static void AfterDbInitialized()
		{
			s_log.Debug("Running AfterDbInitialized for plugins...");
			if (s_plugins.Count == 0) return;
			foreach (var kvp in s_plugins)
			{
				var plugin = kvp.Value;
				if (!plugin.IsDbInitializer) continue;
				try
				{
					plugin.AfterDbInitialized();
				}
				catch (Exception ex)
				{
					s_log.Error($"Error in plugin AfterDbInitialized: {plugin.Id}", ex);
				}
			}
		}

		internal static void AfterDbPostProcess()
		{
			s_log.Debug("Running AfterDbPostProcess for plugins...");
			if (s_plugins.Count == 0) return;
			foreach (var kvp in s_plugins)
			{
				var plugin = kvp.Value;
				if (!plugin.IsDbInitializer) continue;
				try
				{
					plugin.AfterDbPostProcess();
				}
				catch (Exception ex)
				{
					s_log.Error($"Error in plugin AfterDbPostProcess: {plugin.Id}", ex);
				}
			}
		}
	}
}
