using HarmonyLib;
using Neutronium.Core.Elements;
using Neutronium.Core.Meta;
using Neutronium.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Neutronium.Core.Patches
{
	internal class ElementPatches
	{
		/// <summary>
		/// Patch the YAML element loading to inject extra elements
		/// </summary>
		[CorePatch(740622)]
		[HarmonyPatch(nameof(ElementLoader), nameof(ElementLoader.CollectElementsFromYAML))]
		private static class ElementLoader_CollectElementsFromYAML_Patch
		{
			private static void Postfix(ref List<ElementData.ElementEntry> __result)
			{
				ElementsManager.OnElementsLoading(ref __result);
			}
		}

		[CorePatch(740622)]
		[HarmonyPatch(typeof(Enum), nameof(Enum.Parse), [ typeof(Type), typeof(string) ])]
		private static class Enum_Parse_Patch
		{
			private static bool Prefix(Type enumType, string value, ref object __result)
			{
				if (enumType != typeof(SimHashes)) return true;
				__result = (SimHashes)KHash.ParseSimHash(value);
				return false;
			}
		}

		[CorePatch(740622)]
		[HarmonyPatch(typeof(Enum), nameof(Enum.ToString), [])]
		private static class Enum_ToString_Patch
		{
			private static bool Prefix(Enum __instance, ref string __result)
			{
				if (__instance is not SimHashes hash) return true;
				__result = KHash.SimHashToString((int)hash)!; // ONI does not build with nullable enforced, hash of null is 0.
				if (__result == null) return true;
				return false;
			}
		}
	}
}
