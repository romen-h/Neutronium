using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Constants
{
	/// <summary>
	/// Provides int constants for the element states in Oxygen Not Included.
	/// </summary>
	[PreviewApi]
	public static class ElementStates
	{
		[PreviewApi]
		public const int Vacuum = 0;

		[PreviewApi]
		public const int Gas = 1;

		[PreviewApi]
		public const int Liquid = 2;

		[PreviewApi]
		public const int Solid = 3;
		
		/// <summary>
		/// Returns whether this integer value is valid as an element state. 
		/// </summary>
		public static bool IsValidElementState(this int state)
		{
			switch (state)
			{
				case ElementStates.Vacuum:
				case ElementStates.Solid:
				case ElementStates.Liquid:
				case ElementStates.Gas:
					return true;
				default:
					return false;
			}
		}
		
		public static string ToStateName(this int state)
		{
			switch (state)
			{
				case ElementStates.Solid: return "Solid";
				case ElementStates.Liquid: return "Liquid";
				case ElementStates.Gas: return "Gas";
				default: return $"InvalidState{state}";
			}
		}
	}
}
