using ElementData;
using Neutronium.Api.Constants;
using Neutronium.Api.Elements;
using Neutronium.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Game;

namespace Neutronium.Core.Elements
{
	internal class Element : IElement
	{
		/// <inheritdoc/>
		public string Id
		{ get; private set; }

		/// <inheritdoc/>
		public int Hash
		{ get; private set; }
		
		public string? GroupName
		{ get; private set; }

		/// <inheritdoc/>
		public int State
		{ get; private set; }
		
		public bool IsSolid => State == ElementStates.Solid;
		
		public bool IsLiquid => State == ElementStates.Liquid;
		
		public bool IsGas => State == ElementStates.Gas;

		/// <inheritdoc/>
		public string ModId
		{ get; private set; }

		/// <inheritdoc/>
		public string RequiredDlc
		{ get; private set; }

		/// <inheritdoc/>
		public string BaseElementId
		{ get; private set; }

		/// <inheritdoc/>
		public Color32 WorldColor
		{ get; private set; }

		/// <inheritdoc/>
		public Color32 UIColor
		{ get; private set; }

		/// <inheritdoc/>
		public Color32 ConduitColor
		{ get; private set; }

		/// <inheritdoc/>
		public string? MaterialCategory
		{ get; private set; }

		/// <inheritdoc/>
		public IReadOnlyList<string> Tags
		{ get; private set; }

		/// <inheritdoc/>
		public float MaxMass
		{ get; private set; }

		/// <inheritdoc/>
		public float MolarMass
		{ get; private set; }

		/// <inheritdoc/>
		public float SpecificHeatCapacity
		{ get; private set; }

		/// <inheritdoc/>
		public float ThermalConductivity
		{ get; private set; }

		/// <inheritdoc/>
		public float SolidSurfaceAreaMultiplier
		{ get; private set; }

		/// <inheritdoc/>
		public float LiquidSurfaceAreaMultiplier
		{ get; private set; }

		/// <inheritdoc/>
		public float GasSurfaceAreaMultiplier
		{ get; private set; }

		/// <inheritdoc/>
		public float? LowTransitionTempKelvin
		{ get; private set; }

		/// <inheritdoc/>
		public string? LowTransitionElementId
		{ get; private set; }

		/// <inheritdoc/>
		public string? LowTransitionOreId
		{ get; private set; }

		/// <inheritdoc/>
		public float LowTransitionOreMassConversion
		{ get; private set; }

		/// <inheritdoc/>
		public float? HighTransitionTempKelvin
		{ get; private set; }

		/// <inheritdoc/>
		public string? HighTransitionElementId
		{ get; private set; }

		/// <inheritdoc/>
		public string? HighTransitionOreId
		{ get; private set; }

		/// <inheritdoc/>
		public float HighTransitionOreMassConversion
		{ get; private set; }

		/// <inheritdoc/>
		public string? SublimateTargetId
		{ get; private set; }

		/// <inheritdoc/>
		public float SublimateRate
		{ get; private set; }

		/// <inheritdoc/>
		public float SublimateEfficiency
		{ get; private set; }

		/// <inheritdoc/>
		public float SublimateProbability
		{ get; private set; }

		/// <inheritdoc/>
		public string? SublimateFx
		{ get; private set; }

		/// <inheritdoc/>
		public float DefaultMass
		{ get; private set; }

		/// <inheritdoc/>
		public float DefaultTemperatureKelvin
		{ get; private set; }

		/// <inheritdoc/>
		public float LightAbsorptionFactor
		{ get; private set; }

		/// <inheritdoc/>
		public float RadiationAbsorptionFactor
		{ get; private set; }

		/// <inheritdoc/>
		public float RadiationPerKg
		{ get; private set; }

		/// <inheritdoc/>
		public float Strength
		{ get; private set; }

		/// <inheritdoc/>
		public byte Hardness
		{ get; private set; }

		/// <inheritdoc/>
		public float MinHorizontalLiquidFlow
		{ get; private set; }

		/// <inheritdoc/>
		public float MinVerticalLiquidFlow
		{ get; private set; }

		/// <inheritdoc/>
		public float MaxLiquidFlow
		{ get; private set; }

		/// <inheritdoc/>
		public float LiquidCompressionFactor
		{ get; private set; }

		/// <inheritdoc/>
		public float DefaultPressure
		{ get; private set; }

		/// <inheritdoc/>
		public float MaxGasFlow
		{ get; private set; }
		
		internal Substance? Substance
		{ get; set; }

		internal Element(ElementEntry kleiElement)
		{
			if (kleiElement == null) throw new ArgumentNullException(nameof(kleiElement));
			
			Id = kleiElement.elementId;
			Hash = Id.ToKHash();
			GroupName = null;
			State = (int)kleiElement.state;
			ModId = kleiElement.dlcId ?? "";
			RequiredDlc = kleiElement.dlcId ?? "";
			BaseElementId = Id;
			MaterialCategory = kleiElement.materialCategory;
			Tags = kleiElement.tags;
			MaxMass = kleiElement.maxMass;
			MolarMass = kleiElement.molarMass;
			SpecificHeatCapacity = kleiElement.specificHeatCapacity;
			ThermalConductivity = kleiElement.thermalConductivity;
			SolidSurfaceAreaMultiplier = kleiElement.solidSurfaceAreaMultiplier;
			LiquidSurfaceAreaMultiplier = kleiElement.liquidSurfaceAreaMultiplier;
			GasSurfaceAreaMultiplier = kleiElement.gasSurfaceAreaMultiplier;
			LowTransitionTempKelvin = kleiElement.lowTemp;
			LowTransitionElementId = kleiElement.lowTempTransitionTarget;
			LowTransitionOreId = kleiElement.lowTempTransitionOreId;
			LowTransitionOreMassConversion = kleiElement.lowTempTransitionOreMassConversion;
			HighTransitionTempKelvin = kleiElement.highTemp;
			HighTransitionElementId = kleiElement.highTempTransitionTarget;
			HighTransitionOreId = kleiElement.highTempTransitionOreId;
			HighTransitionOreMassConversion = kleiElement.highTempTransitionOreMassConversion;
			SublimateTargetId = kleiElement.sublimateId;
			SublimateRate = State == ElementStates.Solid ? kleiElement.sublimateRate : kleiElement.offGasPercentage;
			SublimateEfficiency = kleiElement.sublimateEfficiency;
			SublimateProbability = kleiElement.sublimateProbability;
			SublimateFx = kleiElement.sublimateFx;
			DefaultMass = kleiElement.defaultMass;
			DefaultTemperatureKelvin = kleiElement.defaultTemperature;
			LightAbsorptionFactor = kleiElement.lightAbsorptionFactor;
			RadiationAbsorptionFactor = kleiElement.radiationAbsorptionFactor;
			RadiationPerKg = kleiElement.radiationPer1000Mass;
			Strength = kleiElement.strength;
			Hardness = kleiElement.hardness;
			MinHorizontalLiquidFlow = kleiElement.minHorizontalFlow;
			MinVerticalLiquidFlow = kleiElement.minVerticalFlow;
			MaxLiquidFlow = kleiElement.speed;
			LiquidCompressionFactor = kleiElement.liquidCompression;
			DefaultPressure = kleiElement.defaultPressure;
			MaxGasFlow = kleiElement.flow;
			
			Validate();
		}
		
		internal Element(ElementBuilderData data, Element baseElement)
		{
			if (data == null) throw new ArgumentNullException(nameof(data));
			if (baseElement == null) throw new ArgumentNullException(nameof(baseElement));
			if (string.IsNullOrWhiteSpace(data.baseElementId)) throw new Exception("Base element id is null or empty.");
			if (data.baseElementId != baseElement.Id) throw new Exception($"Required baseElementId ({data.baseElementId}) and provided baseElement ({baseElement.Id}) do not match.");
			
			Id = data.id;
			Hash = data.hash;
			GroupName = data.groupName;
			State = data.state;
			ModId = data.modId ?? baseElement.ModId;
			RequiredDlc = data?.requiredDlc ?? baseElement.RequiredDlc;
			BaseElementId = baseElement.Id;
			MaterialCategory = data?.materialCategory ?? baseElement.MaterialCategory;
			Tags = (data?.inheritTags ?? true) ? baseElement.Tags : data.tags;
			MaxMass = data?.maxMass ?? baseElement.MaxMass;
			MolarMass = data?.molarMass ?? baseElement.MolarMass;
			SpecificHeatCapacity = data?.specificHeatCapacity ?? baseElement.SpecificHeatCapacity;
			ThermalConductivity = data?.thermalConductivity ?? baseElement.ThermalConductivity;
			SolidSurfaceAreaMultiplier = data?.solidSurfaceAreaMultiplier ?? baseElement.SolidSurfaceAreaMultiplier;
			LiquidSurfaceAreaMultiplier = data?.liquidSurfaceAreaMultiplier ?? baseElement.LiquidSurfaceAreaMultiplier;
			GasSurfaceAreaMultiplier = data?.gasSurfaceAreaMultiplier ?? baseElement.GasSurfaceAreaMultiplier;
			LowTransitionTempKelvin = data?.lowTransitionTempKelvin ?? baseElement.LowTransitionTempKelvin;
			LowTransitionElementId = data?.lowTransitionElementId ?? baseElement.LowTransitionElementId;
			if (LowTransitionElementId == "")
			{
				LowTransitionElementId = null;
				LowTransitionTempKelvin = null;
			}
			LowTransitionOreId = data?.lowTransitionOreId ?? baseElement.LowTransitionOreId;
			if (LowTransitionOreId == "")
			{
				LowTransitionOreId = null;
			}
			LowTransitionOreMassConversion = data?.lowTransitionOreMassConversion ?? baseElement.LowTransitionOreMassConversion;
			HighTransitionTempKelvin = data?.highTransitionTempKelvin ?? baseElement.HighTransitionTempKelvin;
			HighTransitionElementId = data?.highTransitionElementId ?? baseElement.HighTransitionElementId;
			if (HighTransitionElementId == "")
			{
				HighTransitionElementId = null;
				HighTransitionTempKelvin = null;
			}
			HighTransitionOreId = data?.highTransitionOreId ?? baseElement.HighTransitionOreId;
			if (HighTransitionOreId == "")
			{
				HighTransitionOreId = null;
			}
			HighTransitionOreMassConversion = data?.highTransitionOreMassConversion ?? baseElement.HighTransitionOreMassConversion;
			SublimateTargetId = data?.sublimateTargetId ?? baseElement.SublimateTargetId;
			if (SublimateTargetId == "")
			{
				SublimateTargetId = null;
			}
			SublimateRate = data?.sublimateRate ?? baseElement.SublimateRate;
			SublimateEfficiency = data?.sublimateEfficiency ?? baseElement.SublimateEfficiency;
			SublimateProbability = data?.sublimateProbability ?? baseElement.SublimateProbability;
			SublimateFx = data?.sublimateFx ?? baseElement.SublimateFx;
			if (SublimateTargetId == null || SublimateFx == "")
			{
				SublimateFx = null;
			}
			DefaultMass = data?.defaultMass ?? baseElement.DefaultMass;
			DefaultTemperatureKelvin = data?.defaultTemperatureKelvin ?? baseElement.DefaultTemperatureKelvin;
			LightAbsorptionFactor = data?.lightAbsorptionFactor ?? baseElement.LightAbsorptionFactor;
			RadiationAbsorptionFactor = data?.radiationAbsorptionFactor ?? baseElement.RadiationAbsorptionFactor;
			RadiationPerKg = data?.radiationPerKg ?? baseElement.RadiationPerKg;
			Strength = data?.strength ?? baseElement.Strength;
			Hardness = data?.hardness ?? baseElement.Hardness;
			MinHorizontalLiquidFlow = data?.minHorizontalLiquidFlow ?? baseElement.MinHorizontalLiquidFlow;
			MinVerticalLiquidFlow = data?.minVerticalLiquidFlow ?? baseElement.MinVerticalLiquidFlow;
			MaxLiquidFlow = data?.maxLiquidFlow ?? baseElement.MaxLiquidFlow;
			LiquidCompressionFactor = data?.liquidCompressionFactor ?? baseElement.LiquidCompressionFactor;
			DefaultPressure = data?.defaultPressure ?? baseElement.DefaultPressure;
			MaxGasFlow = data?.maxGasFlow ?? baseElement.MaxGasFlow;
			
			Validate();
		}
		
		private void Validate()
		{
			if (string.IsNullOrWhiteSpace(Id)) throw new Exception("Element Id is null or empty.");
			if (Hash == 0) throw new Exception("Hash is zero.");
			if (ModId == null) throw new Exception("ModId is null or empty.");
			if (!State.IsValidElementState()) throw new Exception("State is not a valid value.");
			//if (State == ElementStates.Solid && MaterialCategory == null) throw new Exception("MaterialCategory is required for solid elements.");
			if (float.IsNaN(MaxMass)) throw new Exception("MaxMass is not a number.");
			if (State != ElementStates.Gas && State != ElementStates.Vacuum && MaxMass <= 0) throw new Exception("MaxMass is negative or zero.");
			if (float.IsNaN(MolarMass)) throw new Exception("MolarMass is not a number.");
			if (MolarMass < 0) throw new Exception("MolarMass is negative.");
			if (float.IsNaN(SpecificHeatCapacity)) throw new Exception("SpecificHeatCapacity is not a number.");
			if (SpecificHeatCapacity < 0) throw new Exception("SpecificHeatCapacity is negative.");
			if (float.IsNaN(ThermalConductivity)) throw new Exception("ThermalConductivity is not a number.");
			if (ThermalConductivity < 0) throw new Exception("ThermalConductivity is negative.");
			if (float.IsNaN(SolidSurfaceAreaMultiplier)) throw new Exception("SolidSurfaceAreaMultiplier is not a number.");
			if (SolidSurfaceAreaMultiplier < 0) throw new Exception("SolidSurfaceAreaMultiplier is negative.");
			if (float.IsNaN(LiquidSurfaceAreaMultiplier)) throw new Exception("LiquidSurfaceAreaMultiplier is not a number.");
			if (LiquidSurfaceAreaMultiplier < 0) throw new Exception("LiquidSurfaceAreaMultiplier is negative.");
			if (float.IsNaN(GasSurfaceAreaMultiplier)) throw new Exception("GasSurfaceAreaMultiplier is not a number.");
			if (GasSurfaceAreaMultiplier < 0) throw new Exception("GasSurfaceAreaMultiplier is negative.");
			if (LowTransitionTempKelvin.HasValue && float.IsNaN(LowTransitionTempKelvin.Value)) throw new Exception("LowTransitionTempKelvin is not a number.");
			if (LowTransitionTempKelvin.HasValue && LowTransitionTempKelvin < 0) throw new Exception("LowTransitionTempKelvin is negative.");
			if (LowTransitionTempKelvin.HasValue && LowTransitionTempKelvin > 10000f) throw new Exception("LowTransitionTempKelvin is above max temperature (10000K).");
			if (float.IsNaN(LowTransitionOreMassConversion)) throw new Exception("LowTransitionOreMassConversion is not a number.");
			if (LowTransitionOreMassConversion < 0) throw new Exception("LowTransitionOreMassConversion is negative.");
			if (HighTransitionTempKelvin.HasValue && float.IsNaN(HighTransitionTempKelvin.Value)) throw new Exception("HighTransitionTempKelvin is not a number.");
			if (HighTransitionTempKelvin.HasValue && HighTransitionTempKelvin < 0) throw new Exception("HighTransitionTempKelvin is negative.");
			if (HighTransitionTempKelvin.HasValue && HighTransitionTempKelvin > 10000f) throw new Exception("HighTransitionTempKelvin is above max temperature (10000K).");
			if (float.IsNaN(HighTransitionOreMassConversion)) throw new Exception("HighTransitionOreMassConversion is not a number.");
			if (HighTransitionOreMassConversion < 0) throw new Exception("HighTransitionOreMassConversion is negative.");
			if (float.IsNaN(SublimateRate)) throw new Exception("SublimateRate is not a number.");
			if (SublimateTargetId != null && SublimateRate <= 0) throw new Exception("SublimateRate is negative or zero.");
			if (float.IsNaN(SublimateEfficiency)) throw new Exception("SublimateEfficiency is not a number.");
			if (SublimateTargetId != null && SublimateEfficiency < 0) throw new Exception("SublimateEfficiency is negative.");
			if (float.IsNaN(SublimateProbability)) throw new Exception("SublimateProbability is not a number.");
			if (SublimateTargetId != null && SublimateProbability < 0) throw new Exception("SublimateProbability is negative.");
			if (float.IsNaN(DefaultMass)) throw new Exception("DefaultMass is not a number.");
			if (State != ElementStates.Gas && State != ElementStates.Vacuum && DefaultMass <= 0) throw new Exception("DefaultMass is negative or zero.");
			if (float.IsNaN(DefaultTemperatureKelvin)) throw new Exception("DefaultTemperature is not a number.");
			if (DefaultTemperatureKelvin < 0) throw new Exception("DefaultTemperatureKelvin is negative.");
			if (DefaultTemperatureKelvin > 10000f) throw new Exception("DefaultTemperatureKelvin is above max temperature (10000K).");
			if (float.IsNaN(LightAbsorptionFactor)) throw new Exception("LightAbsorptionFactor is not a number.");
			if (float.IsNaN(RadiationAbsorptionFactor)) throw new Exception("RadiationAbsorptionFactor is not a number.");
			if (float.IsNaN(RadiationPerKg)) throw new Exception("RadiationPerKg is not a number.");
			if (float.IsNaN(Strength)) throw new Exception("Strength is not a number.");
			if (float.IsNaN(MinHorizontalLiquidFlow)) throw new Exception("MinHorizontalLiquidFlow is not a number.");
			if (float.IsNaN(MinVerticalLiquidFlow)) throw new Exception("MinVerticalLiquidFlow is not a number.");
			if (float.IsNaN(MaxLiquidFlow)) throw new Exception("MaxLiquidFlow is not a number.");
			if (float.IsNaN(LiquidCompressionFactor)) throw new Exception("LiquidCompressionFactor is not a number.");
			if (float.IsNaN(DefaultPressure)) throw new Exception("DefaultPressure is not a number.");
			if (State == ElementStates.Gas && DefaultPressure < 0) throw new Exception("DefaultPressure is negative.");
			if (float.IsNaN(MaxGasFlow)) throw new Exception("MaxGasFlow is not a number.");
		}
		
		internal ElementData.ElementEntry ToElementEntry()
		{
			return new ElementData.ElementEntry()
			{
				elementId = Id,
				specificHeatCapacity = SpecificHeatCapacity,
				thermalConductivity = ThermalConductivity,
				solidSurfaceAreaMultiplier = SolidSurfaceAreaMultiplier,
				liquidSurfaceAreaMultiplier = LiquidSurfaceAreaMultiplier,
				gasSurfaceAreaMultiplier = GasSurfaceAreaMultiplier,
				defaultMass = DefaultMass,
				defaultTemperature = DefaultTemperatureKelvin,
				defaultPressure = DefaultPressure,
				molarMass = MolarMass,
				lightAbsorptionFactor = LightAbsorptionFactor,
				radiationAbsorptionFactor = RadiationAbsorptionFactor,
				radiationPer1000Mass = RadiationPerKg,
				lowTempTransitionTarget = LowTransitionElementId,
				lowTemp = LowTransitionTempKelvin,
				highTempTransitionTarget = HighTransitionElementId,
				highTemp = HighTransitionTempKelvin,
				lowTempTransitionOreId = LowTransitionOreId,
				lowTempTransitionOreMassConversion = LowTransitionOreMassConversion,
				highTempTransitionOreId = HighTransitionOreId,
				highTempTransitionOreMassConversion = HighTransitionOreMassConversion,
				sublimateId = SublimateTargetId,
				sublimateFx = SublimateFx,
				sublimateRate = State == ElementStates.Solid ? SublimateRate : 0.0f,
				offGasPercentage = State == ElementStates.Liquid ? SublimateRate : 0.0f,
				sublimateEfficiency = SublimateEfficiency,
				sublimateProbability = SublimateProbability,
				materialCategory = MaterialCategory,
				tags = Tags.ToArray(),
				isDisabled = false,
				strength = Strength,
				maxMass = MaxMass,
				hardness = Hardness,
				toxicity = 0,
				liquidCompression = LiquidCompressionFactor,
				speed = MaxLiquidFlow,
				minHorizontalFlow = MinHorizontalLiquidFlow,
				minVerticalFlow = MinVerticalLiquidFlow,
				convertId = null,
				flow = MaxGasFlow,
				buildMenuSort = 0,
				state = (global::Element.State)State,
				localizationID = $"STRINGS.ELEMENTS.{Id.ToUpperInvariant()}.NAME",
				dlcId = RequiredDlc,
				refinedMetalTarget = null,
				composition = null,
				description = null
			};
		}
	}
}
