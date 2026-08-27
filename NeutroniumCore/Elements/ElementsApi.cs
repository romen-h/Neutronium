using Neutronium.Api.Constants;
using Neutronium.Api.Elements;
using System;
using System.Collections.Generic;
using System.Text;

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

		public ISolidElementBuilder CreateSolid(string groupName, string basedOnId)
		{
			return new ElementBuilder(groupName, ElementStates.Solid, basedOnId, _modId);
		}

		public ILiquidElementBuilder CreateLiquid(string groupName, string basedOnId)
		{
			return new ElementBuilder(groupName, ElementStates.Liquid, basedOnId, _modId);
		}

		public IGasElementBuilder CreateGas(string groupName, string basedOnId)
		{
			return new ElementBuilder(groupName, ElementStates.Gas, basedOnId, _modId);
		}
	}
}
