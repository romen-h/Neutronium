using HarmonyLib;
using Neutronium.Core.Logging.Internal;
using Neutronium.Core.Patches.Internal;
using System;
using System.Reflection;
using Klei;
using Neutronium.Core.Meta;
using Neutronium.Core.Plugins.Internal;
using Neutronium.Core.Paths.Api;

namespace Neutronium.Core
{
	internal static class Main
	{
		[HarmonyPatch(typeof(KProfilerPlugin), nameof(KProfilerPlugin.InitModule))]
		private static class EntryPatch
		{
			internal static bool Prefix()
			{
				Main.OnUnityInitialized();
				return true;
			}
		}

		internal static void OnEntrypoint()
		{
			// Set the version env var so that NeutroniumLoader can verify that the correct core assembly was loaded.
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			string version = thisAssembly.GetName().Version.ToString();
			Environment.SetEnvironmentVariable("NEUTRONIUM_VERSION", version);

			Log.Initialize();
			Log.Info("Core", $"Neutronium.Core Version {version}");

            LaunchArguments.Initialize();

			Log.Info("Core", "Applying entry patch...");
			try
			{
				// Load a harmless type from the assembly we're patching to make it available
				Harmony earlyHarmony = new Harmony("Neutronium.Core.EntryPatch");
				earlyHarmony.CreateClassProcessor(typeof(EntryPatch)).Patch();
			}
			catch (Exception ex)
			{
				Log.Error("Core", "Failed to apply entry patch.", ex);
			}
		}

		internal static void OnUnityInitialized()
		{
			Log.Info("Core", "Unity is initialized.");
			Log.OnUnityInitialized();

			GameVersion.Initialize();
			FilePaths.Initialize();
			CorePatches.ApplyPatches();
			PluginManager.Initialize();
			
			Log.Info("Core", "Neutronium.Core Loaded.");
		}
	}
}
