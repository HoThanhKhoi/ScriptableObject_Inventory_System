using InventorySystem.Core.DomainModels;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Items;
using System.Collections.Generic;

namespace InventorySystem.Core.Interfaces
{
	public interface IInventoryService
	{
		void InitializeInventory();

		void AddItem(BaseItem item);
		void RemoveItem(BaseItem item);

		//bool CanEquipItem(BaseItem item, string slotId);

		void EquipItem(BaseItem item, SlotIdEnum slotId, int subSlotIndex);
		void UnequipItem(SlotIdEnum slotId, int subSlotId);

		// Query methods
		//bool IsEquipped(string slotId);
		//BaseItem GetEquippedItem(string slotId);
		BaseItem[] GetAllItems();

		// For UI to see what's currently equipped
		SlotModel GetSlot(SlotIdEnum slotId);

		SubSlotModel GetSubSlot(SlotIdEnum slotId, int subSlotId);

		bool SetSubSlotClickedStatus(bool status);

		public bool GetSubSlotClickedStatus();

		public int GetCurrentSubSlotId();
		public void SetCurrentSubSlotId(int subSlotId);

		public SlotIdEnum GetCurrentSlotId();
		public void SetCurrentSlotId(SlotIdEnum slotId);

		public List<BaseItem> GetAllEquippedItems(SlotIdEnum slotId);

		public bool IsItemEquipped(BaseItem item);
	}
}
