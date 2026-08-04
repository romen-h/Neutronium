using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Meta
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public class StableApiAttribute : Attribute
	{
		public readonly Version VersionIntroduced;

		public readonly bool Deprecated;
		
		internal StableApiAttribute(string versionIntroduced, bool deprecated = false)
		{
			VersionIntroduced = Version.Parse(versionIntroduced);
			Deprecated = deprecated;
		}
	}
}
