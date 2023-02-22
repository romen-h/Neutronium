using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using KMod;

using Neutronium.Common;

using UnityEngine;

namespace Neutronium.PostProcessing.LUT
{
    public class LUT_API
    {
        private static LUT_API s_instance = null;

        private bool _debugLogging = false;

        public static LUT_API Setup(Harmony harmony, bool debugLogging = false)
        {
			if (debugLogging) Debug.Log("LUT_API::Setup");
			if (harmony == null) throw new ArgumentNullException(nameof(harmony));

            if (debugLogging) Debug.Log("LUT_API: Installing patches...");
            LUT_Patch.Install(harmony, debugLogging);

            if (debugLogging) Debug.Log("LUT_API: Creating static LUT_API instance...");
            s_instance = new LUT_API(debugLogging);

            return s_instance;
        }

        internal LUT_API(bool debugLogging)
        {
            _debugLogging = debugLogging;
        }

        public bool RegisterLUT(string id, LUTSlot slot, int priority, Texture2D texture)
        {
			if (_debugLogging) Debug.Log("LUT_API::RegisterLUT");
			if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (texture == null) throw new ArgumentNullException(nameof(texture));

            if (_debugLogging) Debug.Log("LUT_API: Grabbing LUT registry...");
            var registry = NeutroniumRegistry_PostProc_LUT.Get();

            if (registry.ContainsKey(id))
            {
                if (_debugLogging) Debug.LogWarning("LUT_API: A LUT is already registered for the given id and slot.");
                return false;
            }

            if (_debugLogging) Debug.Log("LUT_API: Inserting LUT entry in registry...");
            System.Tuple<int, int, Texture2D> entry = new System.Tuple<int, int, Texture2D>((int)slot, priority, texture);
            registry.Add(id, entry);

            Debug.Log($"LUT_API: Registered LUT with id={id}");
            return true;
        }

        public bool UnregisterLUT(string id)
        {
            if (_debugLogging) Debug.Log("LUT_API::UnregisterLUT");
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

			if (_debugLogging) Debug.Log("LUT_API: Grabbing LUT registry...");
			var registry = NeutroniumRegistry_PostProc_LUT.Get();

            if (registry.ContainsKey(id))
            {
                return registry.Remove(id);
            }

            return false;
        }

        internal Texture2D GetDayLUT() => GetLUT(LUTSlot.Day);

        internal Texture2D GetNightLUT() => GetLUT(LUTSlot.Night);

        internal Texture2D GetLUT(LUTSlot slot)
        {
			if (_debugLogging) Debug.Log("LUT_API::GetLUT");

			if (_debugLogging) Debug.Log("LUT_API: Grabbing LUT registry...");
			var registry = NeutroniumRegistry_PostProc_LUT.Get();

            if (_debugLogging) Debug.Log($"LUT_API: Searching for highest priority LUT for slot={slot}");
            System.Tuple<int, int, Texture2D> bestLUT = null;
            foreach (var lut in registry.Values)
            {
                if (lut.Item1 == (int)slot || lut.Item1 == (int)LUTSlot.All)
                {
                    if (bestLUT == null || lut.Item2 > bestLUT.Item2)
                    {
                        bestLUT = lut;
                    }
                }
            }

            if (_debugLogging)
            {
                if (bestLUT == null)
                {
                    Debug.Log($"LUT_API: Did not find LUT for slot={slot}");
                }
                else
                {
                    Debug.Log($"LUT_API: Found LUT with priority={bestLUT.Item2} for slot={slot}");
                }
            }

            return bestLUT?.Item3;
        }
    }
}
