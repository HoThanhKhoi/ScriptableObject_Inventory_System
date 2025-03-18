using System.Collections.Generic;
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
		private readonly Dictionary<SlotIdEnum, EquipmentSlotDefinition> _slotDefinitionList = new Dictionary<SlotIdEnum, EquipmentSlotDefinition>();

		//public int GetCapacity(string slotId)
		//{
		//	if (_slotRestrictions.TryGetValue(slotId, out var restriction))
		//		return restriction.Capacity;
		//	return 1; // default
		//}

		//public bool IsItemAllowedInSlot(ItemCategory category, string slotId)
		//{
		//	if (_slotRestrictions.TryGetValue(slotId, out var restriction))
		//	{
		//		return restriction.AllowedCategories.Contains(category);
		//	}
		//	return false;
		//}

		//public List<ItemCategory> GetAllowedCategories(string slotId)
		//{
		//	if (_slotRestrictions.TryGetValue(slotId, out var restriction))
		//	{
		//		return restriction.AllowedCategories;
		//	}
		//	return new List<ItemCategory>();
		//}

		public SlotIdEnum GetSlotId(EquipmentSlotDefinition slotDefinition)
		{
			return slotDefinition.SlotId;
		}

		public List<ItemCategoryEnum> GetAllowedCategories(EquipmentSlotDefinition slotDefinition)
		{
			return slotDefinition.AllowedCategories;
		}

		public List<ItemCategoryEnum> GetAllowedCategories(SlotIdEnum slotId)
		{
			return _slotDefinitionList[slotId].AllowedCategories;
		}
		

		public int GetCapacity(EquipmentSlotDefinition slotDefinition)
		{
			return slotDefinition.Capacity;
		}

		public int GetCapacity(SlotIdEnum slotId)
		{
			return _slotDefinitionList[slotId].Capacity;
		}

		public void AddSlotDefinition(SlotIdEnum slotId, EquipmentSlotDefinition slotDefinition)
		{
			_slotDefinitionList.Add(slotId, slotDefinition);
		}


	}
}
