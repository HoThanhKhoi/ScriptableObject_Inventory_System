using InventorySystem.Data;
using InventorySystem.Data.Enums;
using System.Collections.Generic;

namespace InventorySystem.Core.Interfaces
{
	public interface IEquipmentSlotService
	{
		void AddSlotDefinition(SlotIdEnum slotId, EquipmentSlotDefinition slotDefinition);
		//bool IsItemAllowedInSlot(ItemCategory category, string slotId);

		SlotIdEnum GetSlotId(EquipmentSlotDefinition slotDefinition);
		
		int GetCapacity(SlotIdEnum slotId);
		int GetCapacity(EquipmentSlotDefinition slotDefinition);
		
		List<ItemCategoryEnum> GetAllowedCategories(SlotIdEnum slotId);
		List<ItemCategoryEnum> GetAllowedCategories(EquipmentSlotDefinition slotDefinition);
	}
}
