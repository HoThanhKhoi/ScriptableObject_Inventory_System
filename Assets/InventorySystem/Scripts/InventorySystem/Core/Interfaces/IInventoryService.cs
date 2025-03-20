using InventorySystem.Core.DomainModels;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Items;

namespace InventorySystem.Core.Interfaces
{
	public interface IInventoryService
	{
		void AddItem(BaseItem item);
		void RemoveItem(BaseItem item);

		//bool CanEquipItem(BaseItem item, string slotId);

		void EquipItem(BaseItem item, SlotIdEnum slotId, int subSlotIndex);
		void UnequipItem(SlotIdEnum slotId, int subSlotIndex);

		// Query methods
		//bool IsEquipped(string slotId);
		//BaseItem GetEquippedItem(string slotId);
		BaseItem[] GetAllItems();

		// For UI to see what's currently equipped
		SlotModel GetSlot(SlotIdEnum slotId);
	}
}
