using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neutronium.Api.Constants;
using Neutronium.Api.Elements;
using Neutronium.Api.Logging;
using Neutronium.Core.Logging;
using Neutronium.Core.Utils;

namespace Neutronium.Core.Elements
{
	internal class ElementGroup
	{
		private readonly ILogger _log;
		
		private readonly HashSet<string> _modIds = new HashSet<string>();
		private readonly Dictionary<string, ElementBuilderData> _solidElements = new Dictionary<string, ElementBuilderData>();
		private readonly Dictionary<string, ElementBuilderData> _liqudElements = new Dictionary<string, ElementBuilderData>();
		private readonly Dictionary<string, ElementBuilderData> _gasElements = new Dictionary<string, ElementBuilderData>();

		internal string GroupName
		{ get; private set; }

		internal ElementBuilderData? SelectedSolidElement
		{ get; private set; }
		
		internal ElementBuilderData? SelectedLiquidElement
		{ get; private set; }
		
		internal ElementBuilderData? SelectedGasElement
		{ get; private set; }
		
		internal ElementGroup(string groupName)
		{
			GroupName = groupName;
			_log = LoggerFactory.GetInternalLogger($"Core.ElementsManager.ElementGroup.{groupName}");
		}

		internal void AddElement(ElementBuilderData data)
		{
			switch (data.state)
			{
				case ElementStates.Gas:
					_gasElements[data.modId] = data;
					_modIds.Add(data.modId);
					break;
				case ElementStates.Liquid:
					_liqudElements[data.modId] = data;
					_modIds.Add(data.modId);
					break;
				case ElementStates.Solid:
					_solidElements[data.modId] = data;
					_modIds.Add(data.modId);
					break;

				default:
					throw new System.NotSupportedException($"Element state {data.state} is not supported.");
			}
		}

		internal void DetermineFinalData(IReadOnlyList<string> modsList)
		{
			_log.Debug("Processing final elements for group...");
			
			// Check for the pathological case that an empty group exists.
			if (_solidElements.Count == 0 &&
			    _liqudElements.Count == 0 &&
			    _gasElements.Count == 0)
			{
				_log.Warn("Group exists but has no solid, liquid, or gas members.");
				SelectedSolidElement = null;
				SelectedLiquidElement = null;
				SelectedGasElement = null;
				return;
			}
			
			// Get the reverse-sorted sub-list of mod Ids that have added to this group
			List<string> sortedMods = new List<string>(_modIds.Count);
			for (int i=0; i<modsList.Count; i++)
			{
				if (_modIds.Contains(modsList[i]))
				{
					sortedMods.Insert(0, modsList[i]);
				}
			}
			
			// Try each mod in order of last-loaded to first-loaded
			ElementBuilderData? solid = null;
			ElementBuilderData? liquid = null;
			ElementBuilderData? gas = null;
			for (int i=0; i<sortedMods.Count; i++)
			{
				string modId = sortedMods[i];
				if (_solidElements.TryGetValue(modId, out var solidElement))
				{
					solid = solidElement;
				}
				if (_liqudElements.TryGetValue(modId, out var liquidElement))
				{
					liquid = liquidElement;
				}
				if (_gasElements.TryGetValue(modId, out var gasElement))
				{
					gas = gasElement;
				}
				
				// Once every element is populated we can stop checking
				if (solid != null && liquid != null && gas != null) break;
			}

			// Check for another pathological case where nothing added anything.
			if (solid == null && liquid == null && gas == null)
			{
				_log.Warn("Group exists but could not decide on any solid, liquid, or gas.");
				SelectedSolidElement = null;
				SelectedLiquidElement = null;
				SelectedGasElement = null;
				return;
			}

			// Check for the easy case where all states that exist came from the same mod.
			string? solidModId = solid?.modId;
			string? liquidModId = liquid?.modId;
			string? gasModId = gas?.modId;
			bool solidLiquidSameMod = solidModId == null || liquidModId == null || solidModId == liquidModId;
			bool liquidGasSameMod = liquidModId == null || gasModId == null || liquidModId == gasModId;
			bool solidGasSameMod = solidModId == null || gasModId == null || solidModId == gasModId;
			if (solidLiquidSameMod && liquidGasSameMod && solidGasSameMod)
			{
				_log.Debug($"Group decided on using all 3 states from {solid.modId}.");
				SelectedSolidElement = solid;
				SelectedLiquidElement = liquid;
				SelectedGasElement = gas;
				return;
			}
			
			if (solid == null)
			{
				_log.Debug("Group does not contain a solid element state.");
			}
			if (liquid == null)
			{
				_log.Debug("Group does not contain a liquid element state.");
			}
			if (gas == null)
			{
				_log.Debug("Group does not contain a gas element state.");
			}
			
			// TODO: Repair transitions because the set is from mixed mods
			
			SelectedSolidElement = solid;
			SelectedLiquidElement = liquid;
			SelectedGasElement = gas;
		}
	}
}
