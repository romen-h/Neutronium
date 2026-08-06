using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Registry;

namespace Neutronium.Core.Registry
{
	internal class Registry : ConcurrentDictionary<string, object>, IRegistry
	{
		public object GetValue(string key)
		{
			if (!TryGetValue(key, out object value)) return null;
			return value;
		}

		public void SetValue(string key, object value)
		{
			this[key] = value;
		}
	}
}
