using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Neutronium.Common;

using UnityEngine;

namespace Neutronium.PostProcessing.LUT
{
    internal sealed class NeutroniumRegistry_PostProc_LUT : NeutroniumRegistry<System.Tuple<int, int, Texture2D>>
    {
        internal static IDictionary<string, System.Tuple<int, int, Texture2D>> Get() => AddOrGet(typeof(NeutroniumRegistry_PostProc_LUT));
    }
}
