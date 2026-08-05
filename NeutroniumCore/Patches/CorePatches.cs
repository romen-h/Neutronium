using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FuzzySharp.Edits;
using HarmonyLib;
using Neutronium.Api.Logging;
using Neutronium.Core.Logging.Api;
using Neutronium.Core.Meta;
using Neutronium.Core.Paths.Api;
using Neutronium.Core.Plugins;

namespace Neutronium.Core.Patches
{
	internal static class CorePatches
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.Patches");
		private static readonly Harmony s_harmony = new Harmony("Neutronium.Core");

		internal static void ApplyPatches()
		{
			if (GameVersion.RunningGameVersion == uint.MaxValue)
			{
				s_log.Error("Patches will not be applied because the current game version was not detected.");
				return;
			}
			
			s_log.Info("Applying patches...");

			Type thisType = typeof(CorePatches);
			foreach (var patchClass in thisType.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static))
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

		[CorePatch(740622)]
		[HarmonyPatch(nameof(Db), nameof(Db.Initialize))]
		internal static class DbInitializers_Db_Initialize_Patch
		{
			internal static bool Prefix()
			{
				PluginManager.BeforeDbInitialized();
				return true;
			}
			
			internal static void Postfix()
			{
				PluginManager.AfterDbInitialized();
			}
		}

		[CorePatch(740622)]
		[HarmonyPatch(nameof(Db), nameof(Db.PostProcess))]
		internal static class DbInitializers_Db_PostProcess_Patch
		{
			internal static void Postfix()
			{
				PluginManager.AfterDbPostProcess();
			}
		}
		
        [CorePatch(740622)]
        [HarmonyPatch(nameof(Util), nameof(Util.GetKleiRootPath))]
        internal static class OverrideRootFolder_Util_GetKleiRootPath_Patch
        {
            internal static bool Prefix(ref string __result)
            {
                __result = FilePaths.ActiveDataFolder;
                return false;
            }
        }

        [CorePatch(740622)]
        [HarmonyPatch(nameof(Util), nameof(Util.RootFolder))]
        internal static class OverrideRootFolder_Util_RootFolder_Patch
        {
            internal static bool Prefix(ref string __result)
            {
                __result = FilePaths.ActiveDataFolder;
                return false;
            }
        }

        [CorePatch(740622, isTranspiler: true)]
		[HarmonyPatch(nameof(Global), nameof(Global.Update))]
		internal static class DisableWorkshopService_Global_Update_Patch
		{
			internal static bool Prepare() => LaunchArguments.DisableWorkshop;
			
			internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> inputInstructions)
			{
				var steamInitMethod = typeof(SteamUGCService).GetMethod(nameof(SteamUGCService.Initialize), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

				int startPos = -1;
				List<CodeInstruction> instructions = new List<CodeInstruction>(inputInstructions);
				for (int i=0; i<instructions.Count; i++)
				{
					var instruction = instructions[i];
					if (instruction.opcode == OpCodes.Call && instruction.OperandIs(steamInitMethod))
					{
						startPos = i;
						break;
					}
				}

				if (startPos >= 0)
				{
					instructions.RemoveRange(startPos, 6);
				}

				return instructions;
			}
		}

		[CorePatch(740622)]
		[HarmonyPatch("KMod.Manager", nameof(KMod.Manager.Sanitize))]
		internal static class DisableWorkshopService_KModManager_Sanitize_Patch
		{
			internal static bool Prepare() => LaunchArguments.DisableWorkshop;

			internal static bool Prefix() => false;
		}
	}
}
