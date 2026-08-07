using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Elements;
using UnityEngine;

namespace Neutronium.Core.Elements
{
	internal class ElementBuilder :
		ISolidElementBuilder,
		ISolidElementMaterialBuilder,
		ILiquidElementBuilder,
		ILiquidElementMaterialBuilder,
		IGasElementBuilder,
		IGasElementMaterialBuilder,
		IElementSubstanceBuilder,
		IElementMassPropertiesBuilder,
		IElementThermalPropertiesBuilder,
		IElementLowTransitionBuilder,
		IElementHighTransitionBuilder,
		IElementSublimationPropertiesBuilder,
		IElementLightPropertiesBuilder,
		IElementRadiationPropertiesBuilder,
		IElementWorldgenPropertiesBuilder,
		IElementTagsBuilder,
		IElementOptionalPropertiesBuilder
	{
		private readonly ElementBuilderData _data;
		
		internal ElementBuilder(ElementState state, string groupName, string baseElementId, string modStaticId)
		{
			_data = new ElementBuilderData(state, groupName, baseElementId, modStaticId);
		}
		
		ISolidElementMaterialBuilder ISolidElementBuilder.InheritSolidProperties()
		{
			throw new NotImplementedException();
		}

		ISolidElementMaterialBuilder ISolidElementBuilder.SetSolidProperties(string? materialCategory, float? strength, byte? hardness)
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder ISolidElementMaterialBuilder.InheritMaterial()
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder IGasElementMaterialBuilder.CustomMaterial(Color32? color)
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder ILiquidElementMaterialBuilder.CustomMaterial(Color32? color, Texture? texture)
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder ISolidElementMaterialBuilder.CustomMaterial(Texture? groundTexture, Texture? shineMaskTexture, Texture? shineNormalTexture,
			float? worldUVScale, float? frequency, Color32? shineColor, Color32? tintColor)
		{
			throw new NotImplementedException();
		}

		ILiquidElementMaterialBuilder ILiquidElementBuilder.InheritLiquidProperties()
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder ILiquidElementBuilder.LiquidProperties(float? minHorizontalFlow, float? minVerticalFlow, float? maxFlow,
			float? liquidCompression)
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder ILiquidElementMaterialBuilder.InheritMaterial()
		{
			throw new NotImplementedException();
		}

		IGasElementMaterialBuilder IGasElementBuilder.InheritGasProperties()
		{
			throw new NotImplementedException();
		}

		IGasElementMaterialBuilder IGasElementBuilder.GasProperties(float? defaultPressure, float? flowRate)
		{
			throw new NotImplementedException();
		}

		IElementSubstanceBuilder IGasElementMaterialBuilder.InheritMaterial()
		{
			throw new NotImplementedException();
		}

		IElementMassPropertiesBuilder IElementSubstanceBuilder.InheritSubstance()
		{
			throw new NotImplementedException();
		}

		IElementMassPropertiesBuilder IElementSubstanceBuilder.SubstanceProperties(Color32? uiColor, Color32? conduitColor, string? oreKanimName,
			string? fallingStartSound, string? fallingStopSound)
		{
			throw new NotImplementedException();
		}

		IElementThermalPropertiesBuilder IElementMassPropertiesBuilder.InheritMassProperties()
		{
			throw new NotImplementedException();
		}

		IElementThermalPropertiesBuilder IElementMassPropertiesBuilder.MassProperties(float? maxMass, float? molarMass)
		{
			throw new NotImplementedException();
		}

		IElementLowTransitionBuilder IElementThermalPropertiesBuilder.InheritThermalProperties()
		{
			throw new NotImplementedException();
		}

		IElementLowTransitionBuilder IElementThermalPropertiesBuilder.ThermalProperties(float? thermalConductivity, float? specificHeatCapacity)
		{
			throw new NotImplementedException();
		}

		IElementLowTransitionBuilder IElementThermalPropertiesBuilder.ThermalProperties(float? thermalConductivity, float? specificHeadCapacity,
			float? solidTransferMultiplier, float? liquidTransferMultiplier, float? gasTransferMultiplier)
		{
			throw new NotImplementedException();
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.InheritLowTransition()
		{
			throw new NotImplementedException();
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.NoLowTransition()
		{
			throw new NotImplementedException();
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.LowTransition(float? temperatureKelvin, string? targetElementId)
		{
			throw new NotImplementedException();
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.LowTransitionWithOre(float? temperatureKelvin, string? targetElementId,
			string? oreElementId, float? oreConversionFactor)
		{
			throw new NotImplementedException();
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.InheritHighTransition()
		{
			throw new NotImplementedException();
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.NoHighTransition()
		{
			throw new NotImplementedException();
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.HighTransition(float? temperatureKelvin, string? targetElementId)
		{
			throw new NotImplementedException();
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.HighTransitionWithOre(float? temperatureKelvin, string? targetElementId,
			string? oreElementId, float? oreConversionFactor)
		{
			throw new NotImplementedException();
		}

		IElementLightPropertiesBuilder IElementSublimationPropertiesBuilder.InheritSublimation()
		{
			throw new NotImplementedException();
		}

		IElementLightPropertiesBuilder IElementSublimationPropertiesBuilder.NoSublimation()
		{
			throw new NotImplementedException();
		}

		IElementLightPropertiesBuilder IElementSublimationPropertiesBuilder.SublimatesTo(string? targetElementId, float? inputPercentage, float? conversionRate,
			float? probability, string? sublimateFx)
		{
			throw new NotImplementedException();
		}

		IElementRadiationPropertiesBuilder IElementLightPropertiesBuilder.InheritLightProperties()
		{
			throw new NotImplementedException();
		}

		IElementRadiationPropertiesBuilder IElementLightPropertiesBuilder.LightProperties(float? absorptionFactor)
		{
			throw new NotImplementedException();
		}

		IElementWorldgenPropertiesBuilder IElementRadiationPropertiesBuilder.InheritRadiationProperties()
		{
			throw new NotImplementedException();
		}

		IElementWorldgenPropertiesBuilder IElementRadiationPropertiesBuilder.NotRadioactive(float? absorptionFactor)
		{
			throw new NotImplementedException();
		}

		IElementWorldgenPropertiesBuilder IElementRadiationPropertiesBuilder.Radioactive(float? absorptionFactor, float? radsPerKg)
		{
			throw new NotImplementedException();
		}

		IElementTagsBuilder IElementWorldgenPropertiesBuilder.InheritWorldgenProperties()
		{
			throw new NotImplementedException();
		}

		IElementTagsBuilder IElementWorldgenPropertiesBuilder.WorldgenProperties(float? defaultMass, float? defaultTemperature)
		{
			throw new NotImplementedException();
		}

		IElementOptionalPropertiesBuilder IElementTagsBuilder.InheritTags()
		{
			throw new NotImplementedException();
		}

		IElementOptionalPropertiesBuilder IElementTagsBuilder.NoTags()
		{
			throw new NotImplementedException();
		}

		IElementOptionalPropertiesBuilder IElementTagsBuilder.WithTags(IEnumerable<string> tags)
		{
			throw new NotImplementedException();
		}

		IElementOptionalPropertiesBuilder IElementOptionalPropertiesBuilder.RequireDlc(string dlcId)
		{
			throw new NotImplementedException();
		}

		IElementOptionalPropertiesBuilder IElementOptionalPropertiesBuilder.RequireDlc(IEnumerable<string> dlcId)
		{
			throw new NotImplementedException();
		}

		string IElementOptionalPropertiesBuilder.Submit()
		{
			throw new NotImplementedException();
		}
	}
}
