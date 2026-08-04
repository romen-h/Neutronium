using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Registry.Internal
{
	internal class Registry
	{
		private readonly ConcurrentDictionary<string, (string, object)> _data = new ConcurrentDictionary<string, (string, object)>();

		internal Registry()
		{ }

		internal bool KeyExists(string key) => _data.ContainsKey(key);

		internal object GetValue(string key)
		{
			if (!_data.TryGetValue(key, out var entry)) return null;
			return entry.Item2;
		}

		internal bool TryGetValue(string key, out object value)
		{
			value = null;
			if (!_data.TryGetValue(key, out var entry)) return false;
			value = entry.Item2;
			return true;
		}

		internal void SetValue(string key, string clientId, object value)
		{
			_data[key] = (clientId, value);
		}
	}
}
