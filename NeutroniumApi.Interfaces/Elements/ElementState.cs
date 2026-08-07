using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Elements
{
	[PreviewApi]
	public enum ElementState : int
	{
		Vacuum = 0,
		Gas = 1,
		Liquid = 2,
		Solid = 3
	}
}
