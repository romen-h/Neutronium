using HarmonyLib;
using System;
using System.Reflection;
using Klei;
using Neutronium.Core.Elements;
using Neutronium.Core.Meta;
using Neutronium.Core.Logging;
using Neutronium.Core.Patches;
using Neutronium.Core.Plugins;
using Neutronium.Core.Paths;
using Neutronium.Core.Registry;

namespace Neutronium.Core
{
	internal static class Main
	{
		internal static bool IsTesting
		{ get; private set; }
		
		internal static Version NeutroniumVersion
		{ get; private set; }
		
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
			NeutroniumVersion = thisAssembly.GetName().Version;
			string version = NeutroniumVersion.ToString();
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
			PatchManager.ApplyPatches();

			// Initialize API back-end before plugins
			RegistryManager.Initialize();
			ElementsManager.Initialize();
			
			// Finally load the early plugins
			PluginManager.Initialize();
			
			Log.Info("Core", "Neutronium.Core Loaded.");
		}
		
		internal static void OnTest()
		{
			IsTesting = true;
			
			// Set the version env var so that NeutroniumLoader can verify that the correct core assembly was loaded.
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			NeutroniumVersion = thisAssembly.GetName().Version;
			string version = NeutroniumVersion.ToString();
			Environment.SetEnvironmentVariable("NEUTRONIUM_VERSION", version);

			Log.Initialize(isTest: true);
			Log.Info("Core", $"Neutronium.Core Version {version}");
			
			ElementsManager.Initialize();
		}
	}
}
