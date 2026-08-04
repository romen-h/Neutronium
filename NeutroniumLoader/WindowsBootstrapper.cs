using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Neutronium.Loader
{
	internal sealed class WindowsBootstrapper : Bootstrapper
	{
		private static readonly string DoorstopLibFileName = "winhttp.dll";
		private static readonly string DoorstopConfigFileName = "doorstop_config.ini";
		private static readonly string DoorstopVersionFileName = ".doorstop_version";

		internal override Version GetDoorstopVersion()
		{
			string gameFolder = GetGameFolder();
			
			string doorstopVersionFile = Path.Combine(gameFolder, DoorstopVersionFileName);
			if (!File.Exists(doorstopVersionFile)) return null;
			Version version;
			try
			{
				string versionStr = File.ReadAllText(doorstopVersionFile);
				version = Version.Parse(versionStr);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to read Doorstop version file.");
				Debug.LogException(ex);
				return null;
			}

			string doorstopLibFile = Path.Combine(gameFolder, DoorstopLibFileName);
			if (!File.Exists(doorstopLibFile)) return null;
			
			string doorstopConfigFile = Path.Combine(gameFolder, DoorstopConfigFileName);
			if (!File.Exists(doorstopConfigFile)) return null;

			return version;
		}

		internal override void InstallDoorstop()
		{
			string distFolder = GetDistFolder();
			string coreLibFileSource = Path.Combine(distFolder, NeutroniumCoreLibFileName);
			
			string platformDistFolder = Path.Combine(distFolder, "doorstop_win");
			string doorstopVersionFileSource = Path.Combine(platformDistFolder, DoorstopVersionFileName);
			string doorstopLibFileSource = Path.Combine(platformDistFolder, DoorstopLibFileName);
			string doorstopConfigFileSource = Path.Combine(platformDistFolder, DoorstopConfigFileName);

			string gameFolder = GetGameFolder();
			string doorstopVersionFileDest = Path.Combine(gameFolder, DoorstopVersionFileName);
			string doorstopLibFileDest = Path.Combine(gameFolder, DoorstopLibFileName);
			string doorstopConfigFileDest = Path.Combine(gameFolder, DoorstopConfigFileName);

			File.Copy(doorstopVersionFileSource, doorstopVersionFileDest, true);
			File.Copy(doorstopLibFileSource, doorstopLibFileDest, true);
			File.Copy(doorstopConfigFileSource, doorstopConfigFileDest, true);
		}

		internal override void UninstallDoorstop()
		{
			string gameFolder = GetGameFolder();
			string doorstopVersionFileDest = Path.Combine(gameFolder, DoorstopVersionFileName);
			string doorstopLibFileDest = Path.Combine(gameFolder, DoorstopLibFileName);
			string doorstopConfigFileDest = Path.Combine(gameFolder, DoorstopConfigFileName);

			if (File.Exists(doorstopVersionFileDest)) File.Delete(doorstopVersionFileDest);
			if (File.Exists(doorstopLibFileDest)) File.Delete(doorstopLibFileDest);
			if (File.Exists(doorstopConfigFileDest)) File.Delete(doorstopConfigFileDest);
		}
	}
}
