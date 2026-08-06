using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Api.Meta
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public class PreviewApiAttribute : Attribute
	{ }
}
