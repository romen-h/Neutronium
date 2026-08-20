using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using FuzzySharp.Edits;
using HarmonyLib;
using Neutronium.Api.Logging;
using Neutronium.Core.Logging;
using Neutronium.Core.Meta;
using Neutronium.Core.Paths;
using Neutronium.Core.Plugins;

namespace Neutronium.Core.Patches
{
	internal static class CorePatches
	{
		[CorePatch(740622)]
		[HarmonyPatch(nameof(Db), nameof(Db.Initialize))]
		private static class DbInitializers_Db_Initialize_Patch
		{
			private static bool Prefix()
			{
				PluginManager.BeforeDbInitialized();
				return true;
			}

			private static void Postfix()
			{
				PluginManager.AfterDbInitialized();
			}
		}

		[CorePatch(740622)]
		[HarmonyPatch(nameof(Db), nameof(Db.PostProcess))]
		private static class DbInitializers_Db_PostProcess_Patch
		{
			private static void Postfix()
			{
				PluginManager.AfterDbPostProcess();
			}
		}
		
        [CorePatch(740622)]
        [HarmonyPatch(nameof(Util), nameof(Util.GetKleiRootPath))]
        private static class OverrideRootFolder_Util_GetKleiRootPath_Patch
        {
	        private static bool Prepare() => LaunchArguments.MoveGameFolder;

			private static bool Prefix(ref string __result)
            {
                __result = FilePaths.ActiveDataFolder;
                return false;
            }
        }

        [CorePatch(740622)]
        [HarmonyPatch(nameof(Util), nameof(Util.RootFolder))]
        private static class OverrideRootFolder_Util_RootFolder_Patch
        {
	        private static bool Prepare() => LaunchArguments.MoveGameFolder;

			private static bool Prefix(ref string __result)
            {
                __result = FilePaths.ActiveDataFolder;
                return false;
            }
        }

        [CorePatch(740622, isTranspiler: true)]
		[HarmonyPatch(nameof(Global), nameof(Global.Update))]
        private static class DisableWorkshopService_Global_Update_Patch
		{
			private static bool Prepare() => LaunchArguments.DisableWorkshop;

			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> inputInstructions)
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
		private static class DisableWorkshopService_KModManager_Sanitize_Patch
		{
			private static bool Prepare() => LaunchArguments.DisableWorkshop;

			private static bool Prefix() => false;
		}
	}
}
