using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Elements;

namespace Neutronium.Core.Elements
{
	internal class ElementsApi : IElementsApi
	{
		internal readonly string _modId;
		
		internal ElementsApi(string modId)
		{
			_modId = modId;
		}
		
		public IElement GetElement(string elementId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IElement> GetAllElements()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IElement> GetAllKleiElements()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IElement> GetAllModElements()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IElement> GetAllModElements(string modStaticId)
		{
			throw new NotImplementedException();
		}

		public ISolidElementBuilder CreateSolid(string id, string basedOnId)
		{
			return new ElementBuilder(ElementState.Solid, id, basedOnId, _modId);
		}

		public ILiquidElementBuilder CreateLiquid(string id, string basedOnId)
		{
			return new ElementBuilder(ElementState.Liquid, id, basedOnId, _modId);
		}

		public IGasElementBuilder CreateGas(string id, string basedOnId)
		{
			return new ElementBuilder(ElementState.Gas, id, basedOnId, _modId);
		}
	}
}
