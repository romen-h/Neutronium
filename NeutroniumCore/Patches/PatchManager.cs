using HarmonyLib;
using Neutronium.Core.Logging;
using Neutronium.Core.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Neutronium.Api.Logging;

namespace Neutronium.Core.Patches
{
	internal class PatchManager
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.PatchManager");
		private static readonly Harmony s_harmony = new Harmony("Neutronium.Core");

		internal static void ApplyPatches()
		{
			if (GameVersion.RunningGameVersion == uint.MaxValue)
			{
				s_log.Error("Patches will not be applied because the current game version was not detected.");
				return;
			}

			s_log.Info("Applying patches...");
			ApplyPatchesFrom(typeof(CorePatches));
			//ApplyPatchesFrom(typeof(ElementPatches));
		}
		
		private static void ApplyPatchesFrom(Type patchParentClass)
		{
			foreach (var patchClass in patchParentClass.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static))
			{
				var corePatchAttr = patchClass.GetCustomAttribute<CorePatchAttribute>();
				if (corePatchAttr == null) continue;

				string patchName = patchClass.Name;

				if (corePatchAttr.MinGameVersion.HasValue)
				{
					if (corePatchAttr.MinGameVersion.Value > GameVersion.RunningGameVersion)
					{
						s_log.Warn($"Patch skipped: {patchName}\nRequires ONI version >= {corePatchAttr.MinGameVersion.Value}.");
						continue;
					}
				}

				if (corePatchAttr.MaxGameVersion.HasValue)
				{
					if (corePatchAttr.MaxGameVersion.Value < GameVersion.RunningGameVersion)
					{
						s_log.Warn($"Patch skipped: {patchName}\nRequires ONI version <= {corePatchAttr.MaxGameVersion.Value}");
						continue;
					}
				}

				Apply(patchClass);
			}
		}

		private static void Apply(Type patchType)
		{
			try
			{
				s_harmony.CreateClassProcessor(patchType).Patch();
				s_log.Debug($"Applied patch: {patchType.Name}");
			}
			catch (Exception ex)
			{
				s_log.Error($"Failed to apply patch: {patchType.Name}", ex);
			}
		}
	}
}
