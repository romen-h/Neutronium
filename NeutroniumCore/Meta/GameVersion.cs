using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Neutronium.Core.Logging.Api;

namespace Neutronium.Core.Meta
{
	internal static class GameVersion
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.GameVersion");
		
		internal static uint CompiledGameVersion
		{ get; private set; } = KleiVersion.ChangeList;
		
		internal static uint RunningGameVersion
		{ get; private set; } = uint.MaxValue;

		internal static void Initialize()
		{
			s_log.Info($"Compiled Version: {CompiledGameVersion}");
			
			var kleiVersionType = typeof(KleiVersion);
			var versionField = kleiVersionType.GetField(nameof(KleiVersion.ChangeList), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			if (versionField == null)
			{
				s_log.Error("Failed to determine game version; ChangeList field not found.");
				return;
			}

			try
			{
				RunningGameVersion = (uint)versionField.GetValue(null);
				s_log.Info($"Running Version: {RunningGameVersion}");
			}
			catch (Exception ex)
			{
				s_log.Error("Failed to determine game version.", ex);
				return;
			}

			if (RunningGameVersion == uint.MaxValue)
			{
				s_log.Error($"Failed to determine game version; Unexpected case.");
				return;
			}

			if (RunningGameVersion > CompiledGameVersion)
			{
				s_log.Warn($"Game has been updated; Neutronium may not function correctly.");
			}
		}
	}
}
