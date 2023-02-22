using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Neutronium.Common
{
	internal abstract class NeutroniumRegistry<T> : MonoBehaviour, IDictionary<string, T>
	{
		private Dictionary<string, T> _registry = new Dictionary<string, T>();

		protected static IDictionary<string, T> AddOrGet(Type registryType)
		{
			var instance = Global.Instance.gameObject.GetComponent(registryType.Name) as IDictionary<string, T>;
			if (instance == null)
			{
				instance = Global.Instance.gameObject.AddComponent(registryType) as IDictionary<string, T>;
			}
			UnityEngine.Debug.Assert(instance != null, "Could not find registry. Please report this error on the GitHub issues.");
			return instance;
		}

		#region IDictionary

		public T this[string key]
		{
			get => _registry[key];
			set => _registry[key] = value;
		}

		public ICollection<string> Keys => _registry.Keys;

		public ICollection<T> Values => _registry.Values;

		public int Count => _registry.Count;

		public bool IsReadOnly => false;

		public void Add(string key, T value) => _registry.Add(key, value);

		public void Add(KeyValuePair<string, T> item) => Add(item);

		public void Clear() => _registry.Clear();

		public bool Contains(KeyValuePair<string, T> item) => ((IDictionary<string, T>)_registry).Contains(item);

		public bool ContainsKey(string key) => _registry.ContainsKey(key);

		public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => ((IDictionary<string, T>)_registry).CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _registry.GetEnumerator();

		public bool Remove(string key) => _registry.Remove(key);

		public bool Remove(KeyValuePair<string, T> item) => _registry.Remove(item.Key);

		public bool TryGetValue(string key, out T value) => _registry.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => _registry.GetEnumerator();

		#endregion
	}

	internal sealed class NeutroniumRegistry_Core_Patches : NeutroniumRegistry<System.Tuple<System.Version, object>>
	{
		internal static IDictionary<string, System.Tuple<System.Version, object>> Get() => AddOrGet(typeof(NeutroniumRegistry_Core_Patches));
	}
}
