using InventorySystem.Data.Items;

namespace InventorySystem.Core.Interfaces
{
	public interface IInventoryService
	{
		void AddItem(BaseItem item);
		void RemoveItem(BaseItem item);

		bool CanEquipItem(BaseItem item, string slotId);

		void EquipItem(BaseItem item, string slotId);
		void UnequipItem(string slotId);

		// Query methods
		bool IsEquipped(string slotId);
		BaseItem GetEquippedItem(string slotId);
		BaseItem[] GetAllItems();
	}
}
