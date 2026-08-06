using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Api.Meta
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class BuilderMethodAttribute : Attribute
	{ }
}
