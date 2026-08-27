using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Utils
{
	/// <summary>
	/// Utility methods and caches for hashing.
	/// </summary>
	internal static class KHash
	{
		// Stores the string that hash to SimHashes values for reverse lookup.
		private static readonly Dictionary<int, string> s_simHashCache = new Dictionary<int, string>();

		internal static int SDBMLower(string? value)
		{
			if (value == null) return 0;
			
			uint num = 0;
			for (int index = 0; index < value.Length; ++index)
			{
				num = (uint)((int)char.ToLowerInvariant(value[index]) + ((int)num << 6) + ((int)num << 16 /*0x10*/)) - num;
			}
			
			return (int)num;
		}

		/// <summary>
		/// Returns the SDBMLower hash for this string.
		/// </summary>
		internal static int ToKHash(this string str) => SDBMLower(str);

		/// <summary>
		/// Ensures that an element ID is in the hash cache.
		/// </summary>
		internal static void CacheElementId(string elementId)
		{
			int hash = SDBMLower(elementId);
			s_simHashCache[hash] = elementId;
		}
		
		/// <summary>
		/// Replacement method for Enum.Parse
		/// </summary>
		internal static int ParseSimHash(string elementId)
		{
			int hash = SDBMLower(elementId);
			s_simHashCache[hash] = elementId;
			return hash;
		}

		/// <summary>
		/// Replacement method for SimHashes.ToString()
		/// </summary>
		internal static string? SimHashToString(int hash)
		{
			if (hash == 0) return null;
			return s_simHashCache.GetValueOrDefault(hash);
		}
	}
}
