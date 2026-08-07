using Neutronium.Api.Elements;
using Neutronium.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutronium.Core.Elements
{
	internal class ElementBuilderData
	{
		internal readonly string id;
		internal readonly int hash;
		internal readonly ElementState state;
		internal readonly string baseElementId;
		internal readonly string modId;
		internal readonly List<string> tags = new();

		internal string? requiredDlc;
		internal string? materialCategory;
		internal float? maxMass;
		internal float? molarMass;
		internal float? specificHeatCapacity;
		internal float? thermalConductivity;
		internal float? solidSurfaceAreaMultiplier;
		internal float? liquidSurfaceAreaMultiplier;
		internal float? gasSurfaceAreaMultiplier;
		internal float? lowTransitionTempKelvin;
		internal string? lowTransitionElementId;
		internal string? lowTransitionOreId;
		internal float? lowTransitionOreMassConversion;
		internal float? highTransitionTempKelvin;
		internal string? highTransitionElementId;
		internal string? highTransitionOreId;
		internal float? highTransitionOreMassConversion;
		internal string? sublimateTargetId;
		internal float? sublimateCellInputMassPercent;
		internal float? sublimateOutputMassMultiplier;
		internal float? sublimateRate;
		internal float? sublimateProbability;
		internal string? sublimateFx;
		internal float? defaultMass;
		internal float? defaultTemperatureKelvin;
		internal float? lightAbsorptionFactor;
		internal float? radiationAbsorptionFactor;
		internal float? radiationPerKg;
		internal float? strength;
		internal byte? hardness;
		internal float? minHorizontalLiquidFlow;
		internal float? minVerticalLiquidFlow;
		internal float? maxLiquidFlow;
		internal float? liquidCompressionFactor;
		internal float? defaultPressure;
		internal float? maxGasFlow;

		internal ElementBuilderData(ElementState state, string groupName, string baseElementId, string modStaticId)
		{
			this.state = state;
			this.id = $"{state}_{groupName}";
			this.hash = id.ToKHash();
			this.baseElementId = baseElementId;
			this.modId = modStaticId;
		}
	}
}
