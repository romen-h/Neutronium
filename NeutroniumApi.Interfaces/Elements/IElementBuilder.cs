using Neutronium.Api.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Api.Elements
{
	[PreviewApi]
	public interface IElementBuilder
	{
		[PreviewApi]
		[BuilderMethod]
		public IElementBuilder WithLowTransition(string targetElementId, float temperatureKelvin);
		
		[PreviewApi]
		[BuilderMethod]
		public IElementBuilder WithHighTransition(string targetElementId, float temperatureKelvin);
	}
}
