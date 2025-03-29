using System.Collections.Generic;
using System.Linq;
using InventorySystem.Core.DomainModels;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data;
using InventorySystem.Data.Enums;
using UnityEngine;

namespace InventorySystem.Core.Managers
{
	// Manages slot definitions & restrictions.
	public class EquipmentSlotManager : IEquipmentSlotService
	{
		//private readonly Dictionary<SlotIdEnum, EquipmentSlotDefinition> _slotDefinitionDictionary = new Dictionary<SlotIdEnum, EquipmentSlotDefinition>();

		private readonly List<EquipmentSlotDefinition> _slotDefinitionList = new List<EquipmentSlotDefinition>();

		public List<ItemCategoryEnum> GetAllowedCategories(SlotIdEnum slotId)
		{
			return _slotDefinitionList.FirstOrDefault(s => s.SlotId == slotId).AllowedCategories;
		}

		public int GetCapacity(SlotIdEnum slotId)
		{
			return _slotDefinitionList.FirstOrDefault(s => s.SlotId == slotId).Capacity;
		}

		public void AddSlotDefinition(SlotIdEnum slotId, EquipmentSlotDefinition slotDefinition)
		{
			_slotDefinitionList.Add(slotDefinition);
		}

		public EquipmentSlotDefinition GetSlotDefinitionByIdFromList(SlotIdEnum slotId)
		{
			return _slotDefinitionList.FirstOrDefault(s => s.SlotId == slotId);
		}
	}
}
