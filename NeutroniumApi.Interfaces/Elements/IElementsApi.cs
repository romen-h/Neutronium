using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

namespace Neutronium.Api.Elements
{
	[PreviewApi]
	public interface IElementsApi
	{
		[PreviewApi]
		IElement GetElement(string elementId);
		
		[PreviewApi]
		IEnumerable<IElement> GetAllElements();
		
		[PreviewApi]
		IEnumerable<IElement> GetAllKleiElements();
		
		[PreviewApi]
		IEnumerable<IElement> GetAllModElements();
		
		[PreviewApi]
		IEnumerable<IElement> GetAllModElements(string modStaticId);
		
		[PreviewApi]
		ISolidElementBuilder CreateSolid(string groupName, string basedOnElement);
		
		[PreviewApi]
		ILiquidElementBuilder CreateLiquid(string groupName, string basedOnElement);
		
		[PreviewApi]
		IGasElementBuilder CreateGas(string groupName, string basedOnElement);
	}
}
