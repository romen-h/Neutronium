using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Neutronium.Common;

namespace Neutronium.Zones
{
	internal sealed class NZoneRegistry : NeutroniumRegistry<object>
	{
		internal static IDictionary<string, object> Get() => AddOrGet(typeof(NZoneRegistry));
	}
}
