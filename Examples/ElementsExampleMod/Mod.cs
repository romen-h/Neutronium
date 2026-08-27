using System;
using HarmonyLib;
using KMod;
using Neutronium.Api;
using Neutronium.Api.Constants;
using Neutronium.Api.Constants.KleiElements;
using Neutronium.MergeLib;
using UnityEngine;

namespace ElementsExampleMod
{
	public class Mod : UserMod2
	{
		// Keep a static, singleton copy of the API root object. Getting it multiple times can lead to subtle errors.
		internal static IApiRoot? API
		{ get; private set; }
		
		// If you want to refer to your custom elements in other neutronium APIs, make some static singleton fields for their "handle" values too.
		private static (string,int)? s_solidSilverElementHandle;
		private static (string,int)? s_liquidSilverElementHandle;
		private static (string,int)? s_gasSilverElementHandle;
		private static (string,int)? s_silverOreElementHandle;
		
		public override void OnLoad(Harmony harmony)
		{
			// Get the API object.
			// Make sure to use your exact static mod id from the mod's yaml file.
			// If this returns null then Neutronium is not working. It shouldn't crash though!
			API = NeutroniumApiClient.GetApi(modStaticId: "Neutronium_ElementsExample");
			
			if (API == null)
			{
				Debug.LogError("ElementsExample: Neutronium API is not available. Elements will not be added.");
				return;
			}

			s_solidSilverElementHandle = API.Elements.CreateSolid("Silver", basedOnElement: KleiSolids.Copper.Id)
				.SetSolidProperties(materialCategory: MaterialCategories.RefinedMetal, strength: 0.8f, hardness: 25)
				.InheritMaterial()
				.InheritSubstance()
				.SetMassProperties(maxMass: 8960f, molarMass: 107.868f)
				.SetThermalProperties(thermalConductivity: 40, specificHeatCapacity: 0.235f)
				.NoLowTransition()
				.SetHighTransition(temperatureKelvin: 1234.15f, targetElementId: KleiLiquids.MoltenCopper.Id)
				.NoSublimation()
				.InheritLightProperties()
				.InheritRadiationProperties()
				.SetWorldgenProperties(defaultMass: 1200, defaultTemperature: 300)
				.SetTags([ "BuildableAny" ])
				.Submit();
		}
	}
}
