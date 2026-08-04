using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using rail;
using UnityEngine;

namespace Neutronium.Loader
{
	internal abstract class Bootstrapper
	{
		private static readonly Version ExpectedDoorstopVersion = new Version(4, 5, 0);
		protected static readonly string NeutroniumCoreLibFileName = "NeutroniumCore.dll";
		private static readonly Version ExpectedCoreVersion = new Version(0, 0, 1);

		public Version DoorstopVersion
		{ get; private set; } = null;

		public bool Running
		{ get; private set; } = false;

		public Version CoreVersion
		{ get; private set; } = null;

		public bool RestartRequired
		{ get; private set; } = false;
		
		internal void EnsureDoorstopInstalled()
		{
			DoorstopVersion = GetDoorstopVersion();
			if (DoorstopVersion == null)
			{
				Debug.Log("NeutroniumLoader: Doorstop is not installed.");
				InstallDoorstop();
				InstallNeutroniumCore();
				RestartRequired = true;
				return;
			}
			else if (DoorstopVersion < ExpectedDoorstopVersion)
			{
				Debug.Log($"NeutroniumLoader: Doorstop version {DoorstopVersion} is outdated. NeutroniumLoader will upgrade to {ExpectedDoorstopVersion}.");
				InstallDoorstop();
				InstallNeutroniumCore();
				RestartRequired = true;
				return;
			}
			else
			{
				#if false
				InstallDoorstop();
				Debug.Log($"NeutroniumLoader: Installing new doorstop files...");
				RestartRequired = true;
				#endif
				Debug.Log($"NeutroniumLoader: Doorstop version {DoorstopVersion} is installed.");
			}
			
			Running = CheckDoorstopRunning();
			if (Running)
			{
				Debug.Log("NeutroniumLoader: Doorstop is installed and running.");
				
				CoreVersion = GetNeutroniumCoreVersion();
				if (CoreVersion == null)
				{
					Debug.Log("NeutroniumLoader: Neutronium Core is not installed.");
					InstallNeutroniumCore();
					RestartRequired = true;
					return;
				}
				else if (CoreVersion < ExpectedCoreVersion)
				{
					Debug.Log($"NeutroniumLoader: Neutronium Core version {CoreVersion} is outdated. NeutroniumLoader will upgrade to {ExpectedCoreVersion}.");
					InstallNeutroniumCore();
					RestartRequired = true;
					return;
				}
				else
				{
					#if DEV
					InstallNeutroniumCore();
					Debug.Log($"NeutroniumLoader: Installing new Neutronium Core build...");
					RestartRequired = true;
					#endif
					
					Debug.Log($"NeutroniumLoader: Neutronium Core version {CoreVersion} detected.");
					return;
				}
			}
			else
			{
				Debug.LogWarning("NeutroniumLoader: Doorstop is installed but did not initialize. Try restarting the game?");
				RestartRequired = true;
			}
		}

		internal bool CheckDoorstopRunning()
		{
			try
			{
				string doorStopEnvVar = Environment.GetEnvironmentVariable("DOORSTOP_INITIALIZED") ?? "";
				return doorStopEnvVar == "TRUE";
			}
			catch (Exception ex)
			{
				Debug.LogError("NeutroniumLoader: Failed to read DOORSTOP_INITIALIZED environment variable.");
				Debug.LogException(ex);
				return false;
			}
		}

		protected virtual string GetGameFolder()
		{
			string gameDataFolder = Application.dataPath;
			return Path.GetDirectoryName(gameDataFolder);
		}
		
		protected string GetDistFolder()
		{
			Assembly thisAssembly = Assembly.GetExecutingAssembly();
			string modFolder = Path.GetDirectoryName(thisAssembly.Location);
			string distFolder = Path.Combine(modFolder, "dist");
			return distFolder;
		}

		internal abstract Version GetDoorstopVersion();

		internal abstract void InstallDoorstop();

		internal abstract void UninstallDoorstop();

		internal Version GetNeutroniumCoreVersion()
		{
			try
			{
				string coreVersionEnvVar = Environment.GetEnvironmentVariable("NEUTRONIUM_VERSION") ?? "";
				return Version.TryParse(coreVersionEnvVar, out Version result) ? result : null;
			}
			catch (Exception ex)
			{
				Debug.LogError("NeutroniumLoader: Failed to read NEUTRONIUM_VERSION environment variable.");
				Debug.LogException(ex);
				return null;
			}
		}

		internal void InstallNeutroniumCore()
		{
			string distFolder = GetDistFolder();
			string coreFolder = Path.Combine(distFolder, "core");
			string gameFolder = GetGameFolder();
			
			foreach (string dllFile in Directory.GetFiles(coreFolder, "*.dll", SearchOption.TopDirectoryOnly))
			{
				string fileName = Path.GetFileName(dllFile);
				string dest = Path.Combine(gameFolder, fileName);
				File.Copy(dllFile, dest, true);
			}
		}

		internal void UninstallNeutroniumCore()
		{
			string distFolder = GetDistFolder();
			string coreFolder = Path.Combine(distFolder, "core");
			string gameFolder = GetGameFolder();
			
			foreach (string dllFile in Directory.GetFiles(coreFolder, "*.dll", SearchOption.TopDirectoryOnly))
			{
				string fileName = Path.GetFileName(dllFile);
				string installedFile = Path.Combine(gameFolder, fileName);
				if (File.Exists(installedFile)) File.Delete(installedFile);
			}
		}
	}
}
