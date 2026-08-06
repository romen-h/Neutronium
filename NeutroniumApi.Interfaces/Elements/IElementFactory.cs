using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Elements
{
	[PreviewApi]
	public interface IElementFactory
	{
		[PreviewApi]
		IElementBuilder CreateElement(string id);
	}
}
