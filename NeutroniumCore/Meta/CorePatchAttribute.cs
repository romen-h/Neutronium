using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Meta
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class CorePatchAttribute : Attribute
	{
		public readonly uint? MinGameVersion;
		public readonly uint? MaxGameVersion;
		public readonly bool IsTranspiler;

		internal CorePatchAttribute(uint minGameVersion, bool isTranspiler = false)
		{
			MinGameVersion = minGameVersion;
			MaxGameVersion = null;
			IsTranspiler = isTranspiler;
		}

		internal CorePatchAttribute(uint minGameVersion, uint maxGameVersion, bool isTranspiler = false)
		{
			MinGameVersion = minGameVersion;
			MaxGameVersion = maxGameVersion;
			IsTranspiler = isTranspiler;
		}
	}
}
