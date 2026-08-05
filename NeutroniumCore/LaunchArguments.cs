using System;
using System.IO;
using HarmonyLib;
using Neutronium.Api.Logging;
using Neutronium.Core.Logging.Api;

namespace Neutronium.Core
{
	internal static class LaunchArguments
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.LaunchArguments");

		internal static bool DisableWorkshop
		{ get; private set; } = true;
		
		internal static void Initialize()
		{
			s_log.Debug("Initializing...");
			string[] args = Environment.GetCommandLineArgs();
			string fullArgs = string.Join(' ', args);
			s_log.Info($"Handling launch arguments: {fullArgs}");
			
			foreach (string arg in args)
			{
				try
				{
					string invariant = arg.ToLowerInvariant();
					if (invariant == "--disable-workshop")
					{
						s_log.Info("Steam workshop is disabled by launch argument.");
						DisableWorkshop = true;
					}
				}
				catch (Exception ex)
				{
					s_log.Error($"Failed to handle argument: {arg}", ex);
				}
			}

			s_log.Info("Handling environment vars...");
			
			string disableWorkshopEnvVar = Environment.GetEnvironmentVariable("NEUTRONIUM_DISABLE_WORKSHOP");
			if (disableWorkshopEnvVar != null)
			{
				if (int.TryParse(disableWorkshopEnvVar, out int value) && value == 1)
				{
					s_log.Info("Steam workshop is disabled by environment var.");
					DisableWorkshop = true;
				}
			}
		}
    }
}
