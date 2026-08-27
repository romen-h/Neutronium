using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;
using UnityEngine;

namespace Neutronium.Api.Elements
{
	[PreviewApi]
	public interface IElement
	{
		[PreviewApi]
		[GetOnce]
		string Id
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		int Hash
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		int State
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		string ModId
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		string RequiredDlc
		{ get; }

		[PreviewApi]
		[GetOnce]
		string BaseElementId
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		Color32 WorldColor
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		Color32 UIColor
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		Color32 ConduitColor
		{ get; }

		[PreviewApi]
		[GetOnce]
		string? MaterialCategory
		{ get; }
		
		[PreviewApi]
		IReadOnlyList<string> Tags
		{ get; }

		[PreviewApi]
		[GetOnce]
		float MaxMass
		{ get; }

		[PreviewApi]
		[GetOnce]
		float MolarMass
		{ get; }

		[PreviewApi]
		[GetOnce]
		float SpecificHeatCapacity
		{ get; }

		[PreviewApi]
		[GetOnce]
		float ThermalConductivity
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		float SolidSurfaceAreaMultiplier
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		float LiquidSurfaceAreaMultiplier
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		float GasSurfaceAreaMultiplier
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		float? LowTransitionTempKelvin
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		string? LowTransitionElementId
		{ get; }

		[PreviewApi]
		[GetOnce]
		string? LowTransitionOreId
		{ get; }

		[PreviewApi]
		[GetOnce]
		float LowTransitionOreMassConversion
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		float? HighTransitionTempKelvin
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		string? HighTransitionElementId
		{ get; }

		[PreviewApi]
		[GetOnce]
		string? HighTransitionOreId
		{ get; }

		[PreviewApi]
		[GetOnce]
		float HighTransitionOreMassConversion
		{ get; }

		[PreviewApi]
		[GetOnce]
		string? SublimateTargetId
		{ get; }

		[PreviewApi]
		[GetOnce]
		float SublimateRate
		{ get; }

		[PreviewApi]
		[GetOnce]
		float SublimateEfficiency
		{ get; }

		[PreviewApi]
		[GetOnce]
		float SublimateProbability
		{ get; }

		[PreviewApi]
		[GetOnce]
		string? SublimateFx
		{ get; }

		[PreviewApi]
		[GetOnce]
		float DefaultMass
		{ get; }

		[PreviewApi]
		[GetOnce]
		float DefaultTemperatureKelvin
		{ get; }

		[PreviewApi]
		[GetOnce]
		float LightAbsorptionFactor
		{ get; }

		[PreviewApi]
		[GetOnce]
		float RadiationAbsorptionFactor
		{ get; }

		[PreviewApi]
		[GetOnce]
		float RadiationPerKg
		{ get; }

		[PreviewApi]
		[GetOnce]
		float Strength
		{ get; }

		[PreviewApi]
		[GetOnce]
		byte Hardness
		{ get; }

		[PreviewApi]
		[GetOnce]
		float MinHorizontalLiquidFlow
		{ get; }

		[PreviewApi]
		[GetOnce]
		float MinVerticalLiquidFlow
		{ get; }

		[PreviewApi]
		[GetOnce]
		float MaxLiquidFlow
		{ get; }

		[PreviewApi]
		[GetOnce]
		float LiquidCompressionFactor
		{ get; }

		[PreviewApi]
		[GetOnce]
		float DefaultPressure
		{ get; }
		
		[PreviewApi]
		[GetOnce]
		float MaxGasFlow
		{ get; }
	}
}
