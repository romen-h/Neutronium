using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Neutronium.Core.Logging.Api;
using Neutronium.Core.Meta;
using Neutronium.Core.Paths.Api.FolderModels;
using UnityEngine;
using ILogger = Neutronium.Core.Logging.Api.ILogger;

namespace Neutronium.Core.Paths.Api
{
	[StableApi(ApiVersions.Alpha_Milestone1)]
	public static class FilePaths
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.FilePaths");

		[StableApi(ApiVersions.Alpha_Milestone1)]
		public static GameFolder GameFolder
        { get; private set; }
		
		[StableApi(ApiVersions.Alpha_Milestone1)]
		public static RootFolder DefaultDataFolder
		{ get; private set; }
		
		[StableApi(ApiVersions.Alpha_Milestone1)]
		public static RootFolder ActiveDataFolder
		{ get; private set; }

		[StableApi(ApiVersions.Alpha_Milestone1)]
		public static string LogFile => Application.consoleLogPath;

		internal static void Initialize()
		{
			s_log.Info("Initializing...");

			string coreAssemblyFile = Assembly.GetExecutingAssembly().Location;
			GameFolder = new GameFolder(Path.GetDirectoryName(coreAssemblyFile));
			s_log.Info($"Game Folder: {GameFolder}");

            // Logic sourced from Util class in ONI 740622
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				string defaultKleiFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Klei");
				string defaultDataFolder = Path.Combine(defaultKleiFolder, "OxygenNotIncluded");
				DefaultDataFolder = new RootFolder(defaultDataFolder);
			}
			else
			{
				DefaultDataFolder = new RootFolder(Application.persistentDataPath);
			}
			s_log.Info($"Original Data Folder: {DefaultDataFolder}");
			
			ActiveDataFolder = new RootFolder(Application.persistentDataPath);
			s_log.Info($"Active Data Folder: {ActiveDataFolder}");
			
			if (!File.Exists(DefaultDataFolder.MovedFlag))
			{
				s_log.Info("Original data folder needs to be migrated to AppData...");
				bool moved = false;
				try
				{
					if (Directory.Exists(ActiveDataFolder))
					{
						s_log.Warn("New data folder already exists.");
					}
					DefaultDataFolder.CopyTo(ActiveDataFolder);
					s_log.Info("Copied game data to new folder.");
					moved = true;
				}
				catch (Exception ex)
				{
					s_log.Error("Failed to move game data.", ex);
				}
				
				if (moved)
				{
					Directory.CreateDirectory(DefaultDataFolder);
					File.WriteAllText(DefaultDataFolder.MovedFlag, $"Game data has been moved to: {ActiveDataFolder}");
					s_log.Info("Created \"OneDrive Fix.txt\" in original data folder.");
				}
			}
		}
	}
}
