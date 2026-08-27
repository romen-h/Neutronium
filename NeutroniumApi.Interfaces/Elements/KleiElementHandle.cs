using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Constants;

namespace Neutronium.Api.Elements
{
	/// <summary>
	/// An implementation of an element handle for exposing Klei element constants in the API.
	/// </summary>
	internal class KleiElementHandle : IElementHandle
	{
		private readonly string _id;
		
		/// <inheritdoc/>
		public string Id => _id;

		/// <inheritdoc/>
		public int Hash
		{ get; private set; }

		/// <inheritdoc/>
		public string GroupName
		{ get; private set; }

		/// <inheritdoc/>
		public int State
		{ get; private set; }
		
		/// <summary>
		/// Defines an element handle for a built-in solid element.
		/// </summary>
		internal static KleiElementHandle Solid(string elementId, string? group = null) => new KleiElementHandle(elementId, CalcSimHash(elementId), group ?? elementId, ElementStates.Solid);
		/// <summary>
		/// Defines an element handle for a built-in liquid element.
		/// </summary>
		internal static KleiElementHandle Liquid(string elementId, string? group = null) => new KleiElementHandle(elementId, CalcSimHash(elementId), group ?? elementId, ElementStates.Liquid);
		/// <summary>
		/// Defines an element handle for a built-in gas element.
		/// </summary>
		internal static KleiElementHandle Gas(string elementId, string? group = null) => new KleiElementHandle(elementId, CalcSimHash(elementId), group ?? elementId, ElementStates.Gas);
		
		public static implicit operator string(KleiElementHandle handle) => handle.Id;
		public static implicit operator (string,int)(KleiElementHandle handle) => (handle.GroupName, handle.State);
		public static implicit operator int(KleiElementHandle handle) => handle.Hash;
		
		private static int CalcSimHash(string? elementId)
		{
			if (elementId == null) return 0;
			uint num = 0;
			for (int index = 0; index < elementId.Length; ++index)
			{
				num = (uint)((int)char.ToLowerInvariant(elementId[index]) + ((int)num << 6) + ((int)num << 16 /*0x10*/)) - num;
			}

			return (int)num;
		}

		internal KleiElementHandle(string elementId, int hash, string groupName, int state)
		{
			_id = elementId;
			Hash = hash;
			GroupName = groupName;
			State = state;
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
