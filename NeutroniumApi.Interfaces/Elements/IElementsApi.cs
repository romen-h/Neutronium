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
		ISolidElementBuilder CreateSolid(string id, string basedOnId);
		
		[PreviewApi]
		ILiquidElementBuilder CreateLiquid(string id, string basedOnId);
		
		[PreviewApi]
		IGasElementBuilder CreateGas(string id, string basedOnId);
	}
}
