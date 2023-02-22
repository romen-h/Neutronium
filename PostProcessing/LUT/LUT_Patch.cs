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

		private readonly Harmony _harmony;

		private LUT_Patch(Harmony harmony)
		{
			_harmony = harmony;
		}

		internal static void Install(Harmony harmony, bool debugLogging)
		{
			if (debugLogging) Debug.Log("LUT_Patch::Install");

			if (debugLogging) Debug.Log("LUT_Patch: Grabbing patches registry...");
			var patches = NeutroniumRegistry_Core_Patches.Get();

			bool overwrite = false;
			if (patches.TryGetValue(Key, out System.Tuple<Version, object> existing))
			{
				if (debugLogging) Debug.Log("LUT_Patch: Found existing patch instance.");
				if (Version > existing.Item1)
				{
					if (debugLogging) Debug.Log("LUT_Patch: Overwriting older patch instance.");
					overwrite = true;
				}
				else
				{
					if (debugLogging) Debug.Log("LUT_Patch: Keep existing patch instance.");
				}
			}
			else
			{
				if (debugLogging) Debug.Log("LUT_Patch: Inserting first patch instance.");
				overwrite = true;
			}

			if (overwrite)
			{
				patches[Key] = new System.Tuple<Version, object>(Version, new LUT_Patch(harmony));
			}

			if (debugLogging) Debug.Log("LUT_Patch: Installing bootstrap patch...");
			try
			{
				MethodInfo method = typeof(KMod.Manager).GetMethod(nameof(KMod.Manager.Load), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (method == null) throw new Exception("Failed to find bootstrap target method.");
				HarmonyMethod postfix = new HarmonyMethod(typeof(LUT_Patch).GetMethod("Manager_Load_Postfix", BindingFlags.Static | BindingFlags.NonPublic));
				if (postfix == null) throw new Exception("Failed to find bootstrap method.");
				var result = harmony.Patch(method, null, postfix);

				if (debugLogging) Debug.Log("LUT_Patch: Bootstrap patch installed.");
			}
			catch (Exception ex)
			{
				Debug.LogWarning("LUT_Patch: Failed to install bootstrap patch.");
				Debug.LogException(ex);
			}
		}

		// Bootstrap Patch
		private static void Manager_Load_Postfix(Content content)
		{
			Debug.Log("LUT_Patch::Manager_Load_Postfix");

			// Strings are loaded after DLLs, so this will run after all mods have registered patch instances
			if (content != Content.Strings) return;

			Debug.Log("LUT_Patch: Grabbing patches registry...");
			var patches = NeutroniumRegistry_Core_Patches.Get();

			if (patches.TryGetValue(Key, out System.Tuple<Version, object> existing))
			{
				Type t = existing.Item2.GetType();
				Debug.Log($"LUT_Patch: Patch instance found. Version={existing.Item1}");
				FieldInfo appliedField = t.GetField(nameof(_applied), BindingFlags.NonPublic | BindingFlags.Instance);
				if (appliedField != null)
				{
					bool applied = (bool)appliedField.GetValue(existing.Item2);
					if (!applied)
					{
						Debug.Log("LUT_Patch: Patch not applied yet.");
						MethodInfo applyMethod = t.GetMethod(nameof(Apply), BindingFlags.NonPublic | BindingFlags.Instance);
						if (applyMethod != null)
						{
							Debug.Log("LUT_Patch: Applying patch...");
							applyMethod.Invoke(existing.Item2, Array.Empty<object>());
						}
						else
						{
							Debug.LogWarning("LUT_Patch: Failed to find patch Apply method.");
						}
					}
					else
					{
						Debug.Log("LUT_Patch: Patch already applied.");
					}
				}
				else
				{
					Debug.LogWarning("LUT_Patch: Failed to find patch applied field.");
				}
			}
			else
			{
				Debug.LogWarning("LUT_Patch: Bootstrapper was installed but no patch instance found.");
			}
		}

		// Apply actual patch
		private void Apply()
		{
			if (_applied) return;

			Debug.Log("LUT_Patch: Applying patch...");
			try
			{
				MethodInfo method = typeof(CameraController).GetMethod("OnPrefabInit", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (method == null) throw new Exception("Failed to find patch target method.");
				HarmonyMethod prefix = new HarmonyMethod(typeof(LUT_Patch).GetMethod("CameraController_OnPrefabInit_Prefix", BindingFlags.Static | BindingFlags.NonPublic));
				if (prefix == null) throw new Exception("Failed to find patch method.");
				_harmony.Patch(method, prefix);
				Debug.Log("LUT_Patch: Applied.");
			}
			catch (Exception ex)
			{
				Debug.LogWarning("LUT_Patch: Failed to apply.");
				Debug.LogException(ex);
			}

			_applied = true;
		}

		// LUT Patch
		private static void CameraController_OnPrefabInit_Prefix(CameraController __instance)
		{
			Debug.Log("LUT_Patch::CameraController_OnPrefabInit_Prefix");
			LUT_API lutManager = new LUT_API(true);

			Texture2D dayLUT = lutManager.GetDayLUT();
			if (dayLUT != null)
			{
				Debug.Log("LUT_Patch: Overwriting day LUT...");
				__instance.dayColourCube = dayLUT;
			}

			Texture2D nightLUT = lutManager.GetNightLUT();
			if (nightLUT != null)
			{
				Debug.Log("LUT_Patch: Overwriting night LUT...");
				__instance.nightColourCube = nightLUT;
			}
		}
	}
}
