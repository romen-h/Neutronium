using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Utils
{
	internal static class KHash
	{
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

		internal static int ToKHash(this string str) => SDBMLower(str);
	}
}
