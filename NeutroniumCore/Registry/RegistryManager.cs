using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Registry
{
	internal static class RegistryManager
	{
		private static readonly ConcurrentDictionary<string, Registry> s_modRegistries = new ConcurrentDictionary<string, Registry>();
		
		internal static readonly Registry GlobalRegistry = new Registry();
		
		internal static Registry GetModRegistry(string modStaticId)
		{
			Registry registry = null;
			if (!s_modRegistries.TryGetValue(modStaticId, out registry))
			{
				registry = new Registry();
				s_modRegistries[modStaticId] = registry;
			}
			return registry;
		}
	}
}
