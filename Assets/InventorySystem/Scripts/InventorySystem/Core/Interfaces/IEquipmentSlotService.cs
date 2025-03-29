using InventorySystem.Data;
using InventorySystem.Data.Enums;
using System.Collections.Generic;

namespace InventorySystem.Core.Interfaces
{
	public interface IEquipmentSlotService
	{
		public List<ItemCategoryEnum> GetAllowedCategories(SlotIdEnum slotId);

		public int GetCapacity(SlotIdEnum slotId);

		public void AddSlotDefinition(SlotIdEnum slotId, EquipmentSlotDefinition slotDefinition);

		public EquipmentSlotDefinition GetSlotDefinitionByIdFromList(SlotIdEnum slotId);
	}
}
