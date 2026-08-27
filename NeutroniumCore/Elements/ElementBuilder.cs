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
		
		internal ElementBuilder(string groupName, int state, string baseElementId, string modStaticId)
		{
			_data = new ElementBuilderData(groupName, state, baseElementId, modStaticId);
		}
		
		ISolidElementMaterialBuilder ISolidElementBuilder.InheritSolidProperties()
		{
			_data.materialCategory = null;
			_data.strength = null;
			_data.hardness = null;
			return this;
		}

		ISolidElementMaterialBuilder ISolidElementBuilder.SetSolidProperties(string? materialCategory, float? strength, byte? hardness)
		{
			_data.materialCategory = materialCategory;
			_data.strength = strength;
			_data.hardness = hardness;
			return this;
		}

		IElementSubstanceBuilder ISolidElementMaterialBuilder.InheritMaterial()
		{
			_data.worldColor = null;
			return this;
		}

		IElementSubstanceBuilder ISolidElementMaterialBuilder.CustomMaterial(
			Texture? groundTexture, Texture? shineMaskTexture, Texture? shineNormalTexture,
			float? worldUVScale, float? frequency, Color32? shineColor, Color32? tintColor)
		{
			return this;
		}

		ILiquidElementMaterialBuilder ILiquidElementBuilder.InheritLiquidProperties()
		{
			_data.minHorizontalLiquidFlow = null;
			_data.minVerticalLiquidFlow = null;
			_data.maxLiquidFlow = null;
			_data.liquidCompressionFactor = null;
			return this;
		}

		ILiquidElementMaterialBuilder ILiquidElementBuilder.SetLiquidProperties(
			float? minHorizontalFlow, float? minVerticalFlow, float? maxFlow, float? liquidCompression)
		{
			_data.minHorizontalLiquidFlow = minHorizontalFlow;
			_data.minVerticalLiquidFlow = minVerticalFlow;
			_data.maxLiquidFlow = maxFlow;
			_data.liquidCompressionFactor = liquidCompression;
			return this;
		}

		IElementSubstanceBuilder ILiquidElementMaterialBuilder.InheritMaterial()
		{
			_data.worldColor = null;
			return this;
		}

		IElementSubstanceBuilder ILiquidElementMaterialBuilder.SetCustomMaterial(Color32? color, Texture? texture)
		{
			_data.worldColor = color;
			return this;
		}

		IGasElementMaterialBuilder IGasElementBuilder.InheritGasProperties()
		{
			_data.defaultPressure = null;
			_data.maxGasFlow = null;
			return this;
		}

		IGasElementMaterialBuilder IGasElementBuilder.SetGasProperties(float? defaultPressure, float? flowRate)
		{
			_data.defaultPressure = defaultPressure;
			_data.maxGasFlow = flowRate;
			return this;
		}

		IElementSubstanceBuilder IGasElementMaterialBuilder.InheritMaterial()
		{
			_data.worldColor = null;
			return this;
		}

		IElementSubstanceBuilder IGasElementMaterialBuilder.SetCustomMaterial(Color32? color)
		{
			_data.worldColor = color;
			return this;
		}

		IElementMassPropertiesBuilder IElementSubstanceBuilder.InheritSubstance()
		{
			_data.worldColor = null;
			_data.uiColor = null;
			_data.conduitColor = null;
			_data.oreKanimName = null;
			return this;
		}

		IElementMassPropertiesBuilder IElementSubstanceBuilder.SetSubstanceProperties(
			Color32? worldColor, Color32? uiColor, Color32? conduitColor, string? oreKanimName, string? fallingStartSound, string? fallingStopSound)
		{
			_data.worldColor = worldColor;
			_data.uiColor = uiColor;
			_data.conduitColor = conduitColor;
			_data.oreKanimName = oreKanimName;
			return this;
		}

		IElementThermalPropertiesBuilder IElementMassPropertiesBuilder.InheritMassProperties()
		{
			_data.maxMass = null;
			_data.molarMass = null;
			return this;
		}

		IElementThermalPropertiesBuilder IElementMassPropertiesBuilder.SetMassProperties(float? maxMass, float? molarMass)
		{
			_data.maxMass = maxMass;
			_data.molarMass = molarMass;
			return this;
		}

		IElementLowTransitionBuilder IElementThermalPropertiesBuilder.InheritThermalProperties()
		{
			_data.thermalConductivity = null;
			_data.specificHeatCapacity = null;
			_data.solidSurfaceAreaMultiplier = null;
			_data.liquidSurfaceAreaMultiplier = null;
			_data.gasSurfaceAreaMultiplier = null;
			return this;
		}

		IElementLowTransitionBuilder IElementThermalPropertiesBuilder.SetThermalProperties(float? thermalConductivity, float? specificHeatCapacity)
		{
			_data.thermalConductivity = thermalConductivity;
			_data.specificHeatCapacity = specificHeatCapacity;
			_data.solidSurfaceAreaMultiplier = null;
			_data.liquidSurfaceAreaMultiplier = null;
			_data.gasSurfaceAreaMultiplier = null;
			return this;
		}

		IElementLowTransitionBuilder IElementThermalPropertiesBuilder.SetThermalProperties(
			float? thermalConductivity, float? specificHeatCapacity, float? solidTransferMultiplier,
			float? liquidTransferMultiplier, float? gasTransferMultiplier)
		{
			_data.thermalConductivity = thermalConductivity;
			_data.specificHeatCapacity = specificHeatCapacity;
			_data.solidSurfaceAreaMultiplier = solidTransferMultiplier;
			_data.liquidSurfaceAreaMultiplier = liquidTransferMultiplier;
			_data.gasSurfaceAreaMultiplier = gasTransferMultiplier;
			return this;
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.InheritLowTransition()
		{
			_data.lowTransitionTempKelvin = null;
			_data.lowTransitionElementId = null;
			_data.lowTransitionOreId = null;
			_data.lowTransitionOreMassConversion = null;
			return this;
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.NoLowTransition()
		{
			_data.lowTransitionTempKelvin = 0;
			_data.lowTransitionElementId = "";
			_data.lowTransitionOreId = "";
			_data.lowTransitionOreMassConversion = 0;
			return this;
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.SetLowTransition(float? temperatureKelvin, string? targetElementId)
		{
			_data.lowTransitionTempKelvin = temperatureKelvin;
			_data.lowTransitionElementId = targetElementId;
			_data.lowTransitionOreId = "";
			_data.lowTransitionOreMassConversion = 0;
			return this;
		}

		IElementHighTransitionBuilder IElementLowTransitionBuilder.SetLowTransitionWithOre(
			float? temperatureKelvin, string? targetElementId, string? oreElementId, float? oreConversionFactor)
		{
			_data.lowTransitionTempKelvin = temperatureKelvin;
			_data.lowTransitionElementId = targetElementId;
			_data.lowTransitionOreId = oreElementId;
			_data.lowTransitionOreMassConversion = oreConversionFactor;
			return this;
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.InheritHighTransition()
		{
			_data.highTransitionTempKelvin = null;
			_data.highTransitionElementId = null;
			_data.highTransitionOreId = null;
			_data.highTransitionOreMassConversion = null;
			return this;
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.NoHighTransition()
		{
			_data.highTransitionTempKelvin = 9999f;
			_data.highTransitionElementId = "";
			_data.highTransitionOreId = "";
			_data.highTransitionOreMassConversion = 0;
			return this;
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.SetHighTransition(float? temperatureKelvin, string? targetElementId)
		{
			_data.highTransitionTempKelvin = temperatureKelvin;
			_data.highTransitionElementId = targetElementId;
			_data.highTransitionOreId = "";
			_data.highTransitionOreMassConversion = 0;
			return this;
		}

		IElementSublimationPropertiesBuilder IElementHighTransitionBuilder.SetHighTransitionWithOre(
			float? temperatureKelvin, string? targetElementId, string? oreElementId, float? oreConversionFactor)
		{
			_data.highTransitionTempKelvin = temperatureKelvin;
			_data.highTransitionElementId = targetElementId;
			_data.highTransitionOreId = oreElementId;
			_data.highTransitionOreMassConversion = oreConversionFactor;
			return this;
		}

		IElementLightPropertiesBuilder IElementSublimationPropertiesBuilder.InheritSublimation()
		{
			_data.sublimateTargetId = null;
			_data.sublimateEfficiency = null;
			_data.sublimateRate = null;
			_data.sublimateProbability = null;
			_data.sublimateFx = null;
			return this;
		}

		IElementLightPropertiesBuilder IElementSublimationPropertiesBuilder.NoSublimation()
		{
			_data.sublimateTargetId = "";
			_data.sublimateEfficiency = 0;
			_data.sublimateRate = 0;
			_data.sublimateProbability = 0;
			_data.sublimateFx = "";
			return this;
		}

		IElementLightPropertiesBuilder IElementSublimationPropertiesBuilder.SublimatesTo(
			string? targetElementId, float? conversionRate, float? efficiency, float? probability, string? sublimateFx)
		{
			_data.sublimateTargetId = targetElementId;
			_data.sublimateRate = conversionRate;
			_data.sublimateEfficiency = efficiency;
			_data.sublimateProbability = probability;
			_data.sublimateFx = sublimateFx;
			return this;
		}

		IElementRadiationPropertiesBuilder IElementLightPropertiesBuilder.InheritLightProperties()
		{
			_data.lightAbsorptionFactor = null;
			return this;
		}

		IElementRadiationPropertiesBuilder IElementLightPropertiesBuilder.LightProperties(float? absorptionFactor)
		{
			_data.lightAbsorptionFactor = absorptionFactor;
			return this;
		}

		IElementWorldgenPropertiesBuilder IElementRadiationPropertiesBuilder.InheritRadiationProperties()
		{
			_data.radiationAbsorptionFactor = null;
			_data.radiationPerKg = null;
			return this;
		}

		IElementWorldgenPropertiesBuilder IElementRadiationPropertiesBuilder.NotRadioactive(float? absorptionFactor)
		{
			_data.radiationAbsorptionFactor = absorptionFactor;
			_data.radiationPerKg = 0;
			return this;
		}

		IElementWorldgenPropertiesBuilder IElementRadiationPropertiesBuilder.Radioactive(float? absorptionFactor, float? radsPerKg)
		{
			_data.radiationAbsorptionFactor = absorptionFactor;
			_data.radiationPerKg = radsPerKg;
			return this;
		}

		IElementTagsBuilder IElementWorldgenPropertiesBuilder.InheritWorldgenProperties()
		{
			_data.defaultMass = null;
			_data.defaultTemperatureKelvin = null;
			return this;
		}

		IElementTagsBuilder IElementWorldgenPropertiesBuilder.SetWorldgenProperties(float? defaultMass, float? defaultTemperature)
		{
			_data.defaultMass = defaultMass;
			_data.defaultTemperatureKelvin = defaultTemperature;
			return this;
		}

		IElementOptionalPropertiesBuilder IElementTagsBuilder.InheritTags()
		{
			_data.inheritTags = true;
			return this;
		}
		
		IElementOptionalPropertiesBuilder IElementTagsBuilder.InheritAndAddTags(IEnumerable<string> tags)
		{
			_data.inheritTags = true;
			foreach (var tag in tags)
			{
				_data.tags.Add(tag);
			}
			return this;
		}

		IElementOptionalPropertiesBuilder IElementTagsBuilder.NoTags()
		{
			_data.inheritTags = false;
			_data.tags.Clear();
			return this;
		}

		IElementOptionalPropertiesBuilder IElementTagsBuilder.SetTags(IEnumerable<string> tags)
		{
			_data.inheritTags = false;
			foreach (var tag in tags)
			{
				_data.tags.Add(tag);
			}
			return this;
		}

		IElementOptionalPropertiesBuilder IElementOptionalPropertiesBuilder.RequireDlc(string dlcId)
		{
			_data.requiredDlc = dlcId;
			return this;
		}

		IElementOptionalPropertiesBuilder IElementOptionalPropertiesBuilder.RequireDlc(IEnumerable<string> dlcId)
		{
			return this;
		}

		(string,int)? IElementOptionalPropertiesBuilder.Submit()
		{
			return ElementsManager.SubmitElement(_data);
		}
	}
}
