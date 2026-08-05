using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Api.Meta
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public class WrapInterfaceAttribute : Attribute
	{ }
}
