using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Neutronium.Api.Logging;
using Neutronium.Core.Elements;
using Neutronium.Core.Logging;
using Neutronium.Core.Meta;
using Neutronium.Core.Paths;
using Neutronium.Core.Plugins;

namespace Neutronium.Core.Patches
{
	internal static class CorePatches
	{
		[CorePatch(740622)]
		[HarmonyPatch("KMod.Mod", "PostLoad")]
		private static class KModMod_PostLoad_Patch
		{
			private static bool s_ranOnce = false;
			
			private static bool Prefix(IReadOnlyList<KMod.Mod> mods)
			{
				if (!s_ranOnce)
				{
					ElementsManager.AfterModsLoaded(mods);
					s_ranOnce = true;
				}
				return true;
			}
		}
		
#if false
		[CorePatch(740622)]
		[HarmonyPatch("KMod.Manager", "Load")]
		private static class KModManager_Load_Patch
		{
			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> inputInstructions)
			{
				FieldInfo? modsField = typeof(KMod.Manager).GetField(nameof(KMod.Manager.mods), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				MethodInfo? injectedMethod = typeof(KModManager_Load_Patch).GetMethod(nameof(InjectedMethod), BindingFlags.Static | BindingFlags.NonPublic);
				
				int startPos = -1;
				List<CodeInstruction> instructions = new List<CodeInstruction>(inputInstructions);
				for (int i = 0; i < instructions.Count; i++)
				{
					var instruction = instructions[i];
					if (instruction.opcode == OpCodes.Ldfld && instruction.OperandIs(modsField))
					{
						startPos = i;
						break;
					}
				}

				if (startPos >= 0)
				{
					
					instructions.Insert(startPos + 1, new CodeInstruction(OpCodes.Call, injectedMethod));
					instructions.Insert(startPos + 1, new CodeInstruction(OpCodes.Ldarg_1));
				}

				return instructions;
			}
			
			private static List<KMod.Mod> InjectedMethod(List<KMod.Mod> modsList, int content)
			{
				if (content == 4)
				{
					ElementsManager.AfterModsLoaded(modsList);
				}
				return modsList;
			}
		}
#endif
		
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
