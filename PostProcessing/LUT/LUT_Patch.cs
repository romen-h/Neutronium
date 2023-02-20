using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using KMod;

using Neutronium.Common;
using UnityEngine;

using static ProcGen.SampleDescriber;

namespace Neutronium.PostProcessing.LUT
{
	internal class LUT_Patch
	{
		internal static readonly string Key = "PostProcessing.LUT_Patch";

		internal static readonly Version Version = new Version(1, 0, 0);

		private bool _applied = false;

		private LUT_Patch()
		{ }

		internal static void Install(Harmony harmony)
		{
			var patches = NeutroniumRegistry_Core_Patches.Get();

			bool overwrite = false;

			if (patches.TryGetValue(Key, out Tuple<Version, object> existing))
			{
				if (Version > existing.first)
				{
					overwrite = true;
				}
			}
			else
			{
				overwrite = true;
			}

			if (overwrite)
			{
				patches[Key] = new Tuple<Version, object>(Version, new LUT_Patch());
			}

			try
			{
				MethodInfo method = typeof(Mod).GetMethod(nameof(Mod.PostLoad), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				HarmonyMethod postfix = new HarmonyMethod(typeof(LUT_Patch).GetMethod("Mod_PostLoad_Postfix", BindingFlags.Static | BindingFlags.NonPublic));
				harmony.Patch(method, null, postfix);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Neutronium.PostProcessing: Failed to apply LUTPatch.");
				Debug.LogException(ex);
			}
		}

		private static void Mod_PostLoad_Postfix(LoadedModData modData)
		{
			var patches = NeutroniumRegistry_Core_Patches.Get();

			if (patches.TryGetValue(Key, out Tuple<Version, object> existing))
			{
				Type t = existing.GetType();
				FieldInfo appliedField = t.GetField(nameof(_applied), BindingFlags.NonPublic | BindingFlags.Instance);
				if (appliedField != null)
				{
					bool applied = (bool)appliedField.GetValue(existing);
					if (!applied)
					{
						MethodInfo applyMethod = t.GetMethod(nameof(ApplyPatch), BindingFlags.NonPublic | BindingFlags.Instance);
						if (applyMethod != null)
						{
							applyMethod.Invoke(existing, new object[] { modData.harmony });
						}
					}
				}
			}
		}

		private void ApplyPatch(Harmony harmony)
		{
			if (_applied) return;

			try
			{
				MethodInfo method = typeof(CameraController).GetMethod("OnPrefabInit", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				HarmonyMethod prefix = new HarmonyMethod(typeof(LUT_Patch).GetMethod("CameraController_OnPrefabInit_Prefix", BindingFlags.Static | BindingFlags.NonPublic));
				harmony.Patch(method, prefix);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Neutronium.PostProcessing: Failed to apply LUTPatch.");
				Debug.LogException(ex);
			}

			_applied = true;
		}

		private static void CameraController_OnPrefabInit_Prefix(CameraController __instance)
		{
			LUT_API lutManager = new LUT_API();

			Texture2D dayLUT = lutManager.GetDayLUT();
			if (dayLUT != null)
			{
				__instance.dayColourCube = dayLUT;
			}

			Texture2D nightLUT = lutManager.GetNightLUT();
			if (nightLUT != null)
			{
				__instance.nightColourCube = nightLUT;
			}
		}
	}
}
