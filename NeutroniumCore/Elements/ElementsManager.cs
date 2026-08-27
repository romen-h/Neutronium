using ElementData;
using Neutronium.Api.Elements;
using Neutronium.Core.Logging;
using Neutronium.Core.Paths;
using Neutronium.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Neutronium.Api.Constants;
using UnityEngine;
using VYaml.Serialization;
using ILogger = Neutronium.Api.Logging.ILogger;

namespace Neutronium.Core.Elements
{
	internal static class ElementsManager
	{
		private static readonly ILogger s_log = LoggerFactory.GetInternalLogger("Core.ElementsManager");
		
		private static readonly Dictionary<string, Element> s_kleiElements = new Dictionary<string, Element>();
		private static readonly Dictionary<string, Substance> s_kleiSubstances = new Dictionary<string, Substance>();
		private static readonly Dictionary<string, ElementGroup> s_elementGroups = new Dictionary<string, ElementGroup>();
		private static readonly Dictionary<string, Element> s_modElements = new Dictionary<string, Element>();
		
		private static IReadOnlyList<string> s_loadedModIds;
		
		internal static void Initialize()
		{
			s_log.Info("Initializing...");

			if (!Main.IsTesting)
			{
				LoadElementsYaml(FilePaths.GameFolder.OxygenNotIncluded_Data.StreamingAssets.Elements.GasYaml);
				LoadElementsYaml(FilePaths.GameFolder.OxygenNotIncluded_Data.StreamingAssets.Elements.LiquidYaml);
				LoadElementsYaml(FilePaths.GameFolder.OxygenNotIncluded_Data.StreamingAssets.Elements.SolidYaml);
				LoadElementsYaml(FilePaths.GameFolder.OxygenNotIncluded_Data.StreamingAssets.Elements.SpecialYaml);
			}
		}
		
		private static void LoadElementsYaml(string file)
		{
			if (!File.Exists(file))
			{
				s_log.Error($"Klei elements yaml file is missing.\nFile: {file}");
				return;
			}
			
			string currentId = "";
			try
			{
				s_log.Debug($"Loading elements from {file}...");
				byte[] fileBytes = File.ReadAllBytes(file);
				var collection = YamlSerializer.Deserialize<ElementEntryCollection>(fileBytes, KYaml.Options);
				for (int i=0; i<collection.elements.Length; i++)
				{
					ElementEntry entry = collection.elements[i];
					currentId = entry.elementId;
					s_kleiElements[entry.elementId] = new Element(entry);
					s_log.Debug($"Loaded element {entry.elementId} from Klei yaml.");
				}
			}
			catch (Exception ex)
			{
				s_log.Error($"Failed to parse Klei elements yaml.\nFile: {file}\nElement Id: {currentId}", ex);
			}
		}
		
		internal static (string,int)? SubmitElement(ElementBuilderData data)
		{
			if (!s_kleiElements.ContainsKey(data.baseElementId))
			{
				s_log.Error($"Submitted element is based on a Klei element ID that does not exist.\nMod Id: {data.modId}\nElement Id: {data.id}\nBase Id: {data.baseElementId}");
				return null;
			}
			
			// Ensure a group exists
			string groupName = data.groupName;
			if (!s_elementGroups.TryGetValue(groupName, out ElementGroup group))
			{
				group = new ElementGroup(groupName);
				s_elementGroups[groupName] = group;
			}
			
			// Add to the group
			group.AddElement(data);
			
			// Return the handle so the realized element can be retrieved with that later
			return data.handle;
		}
		
		internal static void AfterModsLoaded(IReadOnlyList<KMod.Mod> modsList)
		{
			s_log.Info("Collecting loaded mod ids...");
			s_loadedModIds = modsList.Where(m => m.IsEnabledForActiveDlc()).Select(m => m.staticID).ToArray();
		}

		internal static void OnElementsLoading(ref List<ElementEntry> entries)
		{
			if (s_elementGroups.Count == 0)
			{
				s_log.Info("No elements were registered.");
				return;
			}

			s_log.Info("Collecting default substances...");
			var vanillaSubstances = Assets.instance.substanceTable.GetList();
			for (int i = 0; i < vanillaSubstances.Count; i++)
			{
				var substance = vanillaSubstances[i];
				if (s_kleiElements.TryGetValue(substance.name, out Element kleiElement))
				{
					kleiElement.Substance = substance;
				}
			}

			if (DlcManager.IsExpansion1Active())
			{
				var spacedOutSubstances = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable.GetList();
				for (int i = 0; i < spacedOutSubstances.Count; i++)
				{
					var substance = spacedOutSubstances[i];
					if (s_kleiElements.TryGetValue(substance.name, out Element kleiElement))
					{
						kleiElement.Substance = substance;
					}
				}
			}

			s_log.Info("Processing registered elements...");
			foreach (var group in s_elementGroups.Values)
			{
				group.DetermineFinalData(s_loadedModIds);

				FinalizeElement(group.SelectedSolidElement);
				FinalizeElement(group.SelectedLiquidElement);
				FinalizeElement(group.SelectedGasElement);
			}

			foreach (var finalElement in s_modElements.Values)
			{
				if (!EnsureAllIdsExist(finalElement, out string? missingId))
				{
					s_log.Error($"Element references an element that does not exist.\nMod Id: {finalElement.ModId}\nElement Id: {finalElement.Id}\nDepends On: {missingId}");
					// TODO: How to handle this?
				}
			}

			s_log.Info("Injecting custom elements...");
			foreach (Element element in s_modElements.Values)
			{
				try
				{
					s_log.Debug($"Adding element id {element.Id} from mod {element.ModId}...");
					ElementEntry entry = element.ToElementEntry();
					if (element.Substance == null) throw new Exception("Element has no substance.");
					vanillaSubstances.Add(element.Substance);
					entries.Add(entry);
					
					string upperId = element.Id.ToUpperInvariant();
					Strings.Add($"STRINGS.ELEMENTS.{upperId}.NAME", element.GroupName);
					Strings.Add($"STRINGS.ELEMENTS.{upperId}.DESC", "");
					Strings.Add($"STRINGS.ELEMENTS.{upperId}.EFFECT", "");
				}
				catch (Exception ex)
				{
					s_log.Error($"Failed to insert custom element.\nMod Id: {element.ModId}\nElement Id: {element.Id}", ex);
				}
			}
		}

		private static bool EnsureAllIdsExist(IElement element, out string? missingId)
		{
			if (!EnsureElementIdExists(element.LowTransitionElementId))
			{
				missingId = element.LowTransitionElementId;
				return false;
			}
			if (!EnsureElementIdExists(element.LowTransitionOreId))
			{
				missingId = element.LowTransitionOreId;
				return false;
			}
			if (!EnsureElementIdExists(element.HighTransitionElementId))
			{
				missingId = element.HighTransitionElementId;
				return false;
			}
			if (!EnsureElementIdExists(element.HighTransitionOreId))
			{
				missingId = element.HighTransitionOreId;
				return false;
			}
			if (!EnsureElementIdExists(element.SublimateTargetId))
			{
				missingId = element.SublimateTargetId;
				return false;
			}
			missingId = null;
			return true;
		}
		
		private static bool EnsureElementIdExists(string? id)
		{
			if (id == null) return true;
			return s_kleiElements.ContainsKey(id) || s_modElements.ContainsKey(id);
		}
		
		private static void FinalizeElement(ElementBuilderData? data)
		{
			if (data == null) return;
			
			s_log.Debug($"Realizing element with id {data.id} from mod {data.modId}...");

			if (!s_kleiElements.TryGetValue(data.baseElementId, out Element kleiBaseElement) || kleiBaseElement == null)
			{
				s_log.Error($"Base element ID missing; Element will not be finalized.\nMod Id: {data.modId}\nElement Id: {data.id}");
				return;
			}

			try
			{
				Element element = new Element(data, kleiBaseElement);
				Substance substance = CreateSubstance(data, kleiBaseElement.Substance);
				element.Substance = substance;
				s_modElements[element.Id] = element;
			}
			catch (Exception ex)
			{
				s_log.Error($"Failed to finalize element.\nMod Id: {data.modId}\nElement Id: {data.id}", ex);
			}
		}
		
		private static Substance CreateSubstance(ElementBuilderData data, Substance kleiBaseSubstance)
		{
			bool isSolid = data.state == ElementStates.Solid;
			
			Material cloneMaterial = new Material(kleiBaseSubstance.material);
			
			KAnimFile animFile = kleiBaseSubstance.anim;
			if (data.oreKanimName != null)
			{
				var anim = Assets.GetAnim(data.oreKanimName);
				if (anim != null)
				{
					animFile = anim;
				}
			}
			
			return new Substance()
			{
				name = data.id,
				nameTag = new Tag(data.id),
				elementID = (SimHashes)data.hash,
				anim = animFile,
				colour = data.worldColor ?? kleiBaseSubstance.colour,
				uiColour = data.uiColor ?? kleiBaseSubstance.uiColour,
				conduitColour = data.conduitColor ?? kleiBaseSubstance.conduitColour,
				material = cloneMaterial,
				renderedByWorld = isSolid
			};
		}
	}
}
