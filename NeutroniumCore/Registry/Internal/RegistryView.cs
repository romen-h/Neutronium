using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Core.Registry.Interfaces;

namespace Neutronium.Core.Registry.Internal
{
	internal class RegistryView : IRegistry
	{
		private readonly string _clientId;
		private readonly Registry _registry;

		internal RegistryView(Registry registry, string clientId)
		{
			_clientId = clientId;
			_registry = registry;
		}

		bool IRegistry.KeyExists(string key) => _registry.KeyExists(key);

		object IRegistry.GetValue(string key) => _registry.GetValue(key);

		bool IRegistry.TryGetValue(string key, out object value) => _registry.TryGetValue(key, out value);

		void IRegistry.SetValue(string key, object value) => _registry.SetValue(key, _clientId, value);
	}
}
