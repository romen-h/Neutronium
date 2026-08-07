using System;
using System.Collections.Generic;
using System.Text;
using Neutronium.Api.Meta;

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
		ElementState State
		{ get; }
		
		[PreviewApi]
		string ModId
		{ get; }
		
		[PreviewApi]
		IEnumerable<string> RequiredDlc
		{ get; }

		[PreviewApi]
		string BaseElementId
		{ get; }

		[PreviewApi]
		string MaterialCategory
		{ get; }
		
		[PreviewApi]
		IEnumerable<string> Tags
		{ get; }

		[PreviewApi]
		float MaxMass
		{ get; }

		[PreviewApi]
		float MolarMass
		{ get; }

		[PreviewApi]
		float SpecificHeatCapacity
		{ get; }

		[PreviewApi]
		float ThermalConductivity
		{ get; }
		
		[PreviewApi]
		float SolidSurfaceAreaMultiplier
		{ get; }
		
		[PreviewApi]
		float LiquidSurfaceAreaMultiplier
		{ get; }
		
		[PreviewApi]
		float GasSurfaceAreaMultiplier
		{ get; }
		
		[PreviewApi]
		float LowTransitionTempKelvin
		{ get; }
		
		[PreviewApi]
		string? LowTransitionElementId
		{ get; }

		[PreviewApi]
		string? LowTransitionOreId
		{ get; }

		[PreviewApi]
		float? LowTransitionOreMassConversion
		{ get; }
		
		[PreviewApi]
		float HighTransitionTempKelvin
		{ get; }
		
		[PreviewApi]
		string? HighTransitionElementId
		{ get; }

		[PreviewApi]
		string? HighTransitionOreId
		{ get; }

		[PreviewApi]
		float? HighTransitionOreMassConversion
		{ get; }

		[PreviewApi]
		string? SublimateTargetId
		{ get; }
		
		[PreviewApi]
		float? SublimateCellInputMassPercent
		{ get; }

		[PreviewApi]
		float? SublimateOutputMassMultiplier
		{ get; }

		[PreviewApi]
		float? SublimateRate
		{ get; }

		[PreviewApi]
		float? SublimateProbability
		{ get; }

		[PreviewApi]
		string? SublimateFx
		{ get; }

		[PreviewApi]
		float? DefaultMass
		{ get; }

		[PreviewApi]
		float? DefaultTemperatureKelvin
		{ get; }

		[PreviewApi]
		float LightAbsorptionFactor
		{ get; }

		[PreviewApi]
		float RadiationAbsorptionFactor
		{ get; }

		[PreviewApi]
		float RadiationPerKg
		{ get; }

		[PreviewApi]
		float? Strength
		{ get; }

		[PreviewApi]
		byte? Hardness
		{ get; }

		[PreviewApi]
		float? MinHorizontalLiquidFlow
		{ get; }

		[PreviewApi]
		float? MinVerticalLiquidFlow
		{ get; }

		[PreviewApi]
		float? MaxLiquidFlow
		{ get; }

		[PreviewApi]
		float? LiquidCompressionFactor
		{ get; }

		[PreviewApi]
		float? DefaultPressure
		{ get; }
		
		[PreviewApi]
		float? MaxGasFlow
		{ get; }
	}
}
