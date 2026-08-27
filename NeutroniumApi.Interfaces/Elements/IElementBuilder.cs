using Neutronium.Api.Meta;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

namespace Neutronium.Api.Elements
{
	[PreviewApi]
	public interface ISolidElementBuilder
	{
		[PreviewApi]
		ISolidElementMaterialBuilder InheritSolidProperties();

		[PreviewApi]
		ISolidElementMaterialBuilder SetSolidProperties(
			string? materialCategory,
			float? strength,
			byte? hardness);
	}
	
	[PreviewApi]
	public interface ISolidElementMaterialBuilder
	{
		[PreviewApi]
		IElementSubstanceBuilder InheritMaterial();

		[PreviewApi]
		IElementSubstanceBuilder CustomMaterial(
			Texture? groundTexture,
			Texture? shineMaskTexture,
			Texture? shineNormalTexture,
			float? worldUVScale,
			float? frequency,
			Color32? shineColor,
			Color32? tintColor);
	}
	
	[PreviewApi]
	public interface ILiquidElementBuilder
	{
		[PreviewApi]
		ILiquidElementMaterialBuilder InheritLiquidProperties();

		[PreviewApi]
		ILiquidElementMaterialBuilder SetLiquidProperties(
			float? minHorizontalFlow,
			float? minVerticalFlow,
			float? maxFlow,
			float? liquidCompression);
	}

	[PreviewApi]
	public interface ILiquidElementMaterialBuilder
	{
		[PreviewApi]
		IElementSubstanceBuilder InheritMaterial();

		[PreviewApi]
		IElementSubstanceBuilder SetCustomMaterial(
			Color32? color,
			Texture? texture);
	}
	
	[PreviewApi]
	public interface IGasElementBuilder
	{
		[PreviewApi]
		IGasElementMaterialBuilder InheritGasProperties();
		
		[PreviewApi]
		IGasElementMaterialBuilder SetGasProperties(
			float? defaultPressure,
			float? flowRate);
	}

	[PreviewApi]
	public interface IGasElementMaterialBuilder
	{
		[PreviewApi]
		IElementSubstanceBuilder InheritMaterial();
		
		[PreviewApi]
		IElementSubstanceBuilder SetCustomMaterial(
			Color32? color);
	}
	
	[PreviewApi]
	public interface IElementSubstanceBuilder
	{
		[PreviewApi]
		IElementMassPropertiesBuilder InheritSubstance();

		[PreviewApi]
		IElementMassPropertiesBuilder SetSubstanceProperties(
			Color32? worldColor,
			Color32? uiColor,
			Color32? conduitColor,
			string? oreKanimName,
			string? fallingStartSound,
			string? fallingStopSound);
	}
	
	[PreviewApi]
	public interface IElementMassPropertiesBuilder
	{
		[PreviewApi]
		IElementThermalPropertiesBuilder InheritMassProperties();

		[PreviewApi]
		IElementThermalPropertiesBuilder SetMassProperties(
			float? maxMass,
			float? molarMass);
	}

	[PreviewApi]
	public interface IElementThermalPropertiesBuilder
	{
		[PreviewApi]
		IElementLowTransitionBuilder InheritThermalProperties();

		[PreviewApi]
		IElementLowTransitionBuilder SetThermalProperties(
			float? thermalConductivity,
			float? specificHeatCapacity);

		[PreviewApi]
		IElementLowTransitionBuilder SetThermalProperties(
			float? thermalConductivity,
			float? specificHeatCapacity,
			float? solidTransferMultiplier,
			float? liquidTransferMultiplier,
			float? gasTransferMultiplier);
	}
	
	[PreviewApi]
	public interface IElementLowTransitionBuilder
	{
		[PreviewApi]
		IElementHighTransitionBuilder InheritLowTransition();

		[PreviewApi]
		IElementHighTransitionBuilder NoLowTransition();

		[PreviewApi]
		IElementHighTransitionBuilder SetLowTransition(
			float? temperatureKelvin,
			string? targetElementId);

		[PreviewApi]
		IElementHighTransitionBuilder SetLowTransitionWithOre(
			float? temperatureKelvin,
			string? targetElementId,
			string? oreElementId,
			float? oreConversionFactor);
	}

	[PreviewApi]
	public interface IElementHighTransitionBuilder
	{
		[PreviewApi]
		IElementSublimationPropertiesBuilder InheritHighTransition();

		[PreviewApi]
		IElementSublimationPropertiesBuilder NoHighTransition();

		[PreviewApi]
		IElementSublimationPropertiesBuilder SetHighTransition(
			float? temperatureKelvin,
			string? targetElementId);

		[PreviewApi]
		IElementSublimationPropertiesBuilder SetHighTransitionWithOre(
			float? temperatureKelvin,
			string? targetElementId,
			string? oreElementId,
			float? oreConversionFactor);
	}
	
	[PreviewApi]
	public interface IElementSublimationPropertiesBuilder
	{
		[PreviewApi]
		IElementLightPropertiesBuilder InheritSublimation();

		[PreviewApi]
		IElementLightPropertiesBuilder NoSublimation();

		[PreviewApi]
		IElementLightPropertiesBuilder SublimatesTo(
			string? targetElementId,
			float? conversionRate,
			float? efficiency,
			float? probability,
			string? sublimateFx);
	}

	[PreviewApi]
	public interface IElementLightPropertiesBuilder
	{
		[PreviewApi]
		IElementRadiationPropertiesBuilder InheritLightProperties();

		[PreviewApi]
		IElementRadiationPropertiesBuilder LightProperties(float? absorptionFactor);
	}

	[PreviewApi]
	public interface IElementRadiationPropertiesBuilder
	{
		[PreviewApi]
		IElementWorldgenPropertiesBuilder InheritRadiationProperties();

		[PreviewApi]
		IElementWorldgenPropertiesBuilder NotRadioactive(float? absorptionFactor);

		[PreviewApi]
		IElementWorldgenPropertiesBuilder Radioactive(
			float? absorptionFactor,
			float? radsPerKg);
	}

	[PreviewApi]
	public interface IElementWorldgenPropertiesBuilder
	{
		[PreviewApi]
		IElementTagsBuilder InheritWorldgenProperties();

		[PreviewApi]
		IElementTagsBuilder SetWorldgenProperties(
			float? defaultMass,
			float? defaultTemperature);
	}

	[PreviewApi]
	public interface IElementTagsBuilder
	{
		[PreviewApi]
		IElementOptionalPropertiesBuilder InheritTags();
		
		[PreviewApi]
		IElementOptionalPropertiesBuilder InheritAndAddTags(IEnumerable<string> tags);

		[PreviewApi]
		IElementOptionalPropertiesBuilder NoTags();
		
		[PreviewApi]
		IElementOptionalPropertiesBuilder SetTags(IEnumerable<string> tags);
	}

	[PreviewApi]
	public interface IElementOptionalPropertiesBuilder
	{
		[PreviewApi]
		IElementOptionalPropertiesBuilder RequireDlc(string dlcId);

		[PreviewApi]
		IElementOptionalPropertiesBuilder RequireDlc(IEnumerable<string> dlcId);

		[PreviewApi]
		(string,int)? Submit();
	}
}
