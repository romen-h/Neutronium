using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Constants;
using Neutronium.Api.Elements;
using Neutronium.Core.Utils;

namespace Neutronium.Core.Elements
{
	internal class ElementHandle : IElementHandle
	{
		private readonly string _id;
		private readonly int _hash;
		private readonly string _groupName;
		private readonly int _state;

		/// <inheritdoc/>
		public string Id => _id;
		/// <inheritdoc/>
		public int Hash => _hash;
		/// <inheritdoc/>
		public string GroupName => _groupName;
		/// <inheritdoc/>
		public int State => _state;

		public static implicit operator string(ElementHandle handle) => handle._id;
		public static implicit operator (string, int)(ElementHandle handle) => (handle._groupName, handle._state);
		public static implicit operator int(ElementHandle handle) => handle._hash;
		public static implicit operator SimHashes(ElementHandle handle) => (SimHashes)handle._hash;

		internal ElementHandle(string groupName, int state)
		{
			_id = $"{groupName}_{state.ToStateName()}";
			_hash = _id.ToKHash();
			_groupName = groupName;
			_state = state;
		}

		/// <inheritdoc/>
		public override string ToString() => _id;

		/// <inheritdoc/>
		public override bool Equals(object? obj)
		{
			if (obj == null) return false;
			if (obj is IElementHandle handle)
			{
				return _id == handle.Id;
			}

			return obj.ToString() == _id;
		}

		/// <inheritdoc/>
		public override int GetHashCode() => _id.GetHashCode();
	}
}
