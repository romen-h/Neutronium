using Neutronium.Api.Elements;
using Neutronium.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Neutronium.Core.Elements
{
	internal class ElementBuilderData
	{
		// Identity
		internal readonly ElementHandle handle;
		internal string id => handle.Id;
		internal int hash => handle.Hash;
		internal string groupName => handle.GroupName;
		internal int state => handle.State;
		internal readonly string baseElementId;
		internal readonly string modId;
		
		// Material
		internal Color32? worldColor;
		
		// Substance
		internal Color32? uiColor;
		internal Color32? conduitColor;
		internal string? oreKanimName;
		
		// Tags
		internal readonly List<string> tags = new();
		internal bool inheritTags = false;
		
		// The Rest...
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
		internal float? sublimateRate;
		internal float? sublimateEfficiency;
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

		internal ElementBuilderData(string groupName, int state, string baseElementId, string modStaticId)
		{
			this.handle = new ElementHandle(groupName, state);
			this.baseElementId = baseElementId;
			this.modId = modStaticId;
		}
	}
}
