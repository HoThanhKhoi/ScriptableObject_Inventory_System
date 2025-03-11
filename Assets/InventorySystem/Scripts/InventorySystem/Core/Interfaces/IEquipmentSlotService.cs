using InventorySystem.Data.Enums;
using System.Collections.Generic;

namespace InventorySystem.Core.Interfaces
{
	public interface IEquipmentSlotService
	{
		void RegisterSlot(string slotId, List<ItemCategory> allowedCategories, int capacity);
		bool IsItemAllowedInSlot(ItemCategory category, string slotId);
		int GetCapacity(string slotId);
	}
}
