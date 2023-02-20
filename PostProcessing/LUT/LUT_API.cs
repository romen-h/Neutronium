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

        public bool EnableLogging
        { get; set; } = false;

        public static LUT_API Setup(Harmony harmony)
        {
            if (harmony == null) throw new ArgumentNullException(nameof(harmony));

            LUT_Patch.Install(harmony);

            s_instance = new LUT_API();

            return s_instance;
        }

        internal LUT_API()
        { }

        public bool RegisterLUT(string id, LUTSlot slot, int priority, Texture2D texture)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (texture == null) throw new ArgumentNullException(nameof(texture));

            var registry = NeutroniumRegistry_PostProc_LUT.Get();

            if (registry.ContainsKey(id))
            {
                if (EnableLogging)
                {
                    Debug.LogWarning("Neutronium.LUTManager: A LUT is already registered for the given id and slot.");
                }
                return false;
            }

            Tuple<LUTSlot, int, Texture2D> entry = new Tuple<LUTSlot, int, Texture2D>(slot, priority, texture);
            registry.Add(id, entry);

            return true;
        }

        public bool UnregisterLUT(string id, LUTSlot slot)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var registry = NeutroniumRegistry_PostProc_LUT.Get();

            if (registry.ContainsKey(id))
            {
                registry.Remove(id);
                return true;
            }

            return false;
        }

        internal Texture2D GetDayLUT() => GetLUT(LUTSlot.Day);

        internal Texture2D GetNightLUT() => GetLUT(LUTSlot.Night);

        internal Texture2D GetLUT(LUTSlot slot)
        {
            var registry = NeutroniumRegistry_PostProc_LUT.Get();

            Tuple<LUTSlot, int, Texture2D> bestLUT = null;

            foreach (var lut in registry.Values)
            {
                if (lut.first == slot || lut.first == LUTSlot.All)
                {
                    if (lut.second > bestLUT.second)
                    {
                        bestLUT = lut;
                    }
                }
            }

            return bestLUT?.third;
        }
    }
}
