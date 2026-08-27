using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Utils
{
	internal static class FuzzyFloatMath
	{
		internal static bool RoughlyEqual(float? a, float? b, float precision = 0.001f)
		{
			if (a == null || b == null) return false;
			
			float diff = Math.Abs(a.Value - b.Value);
			return diff < precision;
		}
	}
}
