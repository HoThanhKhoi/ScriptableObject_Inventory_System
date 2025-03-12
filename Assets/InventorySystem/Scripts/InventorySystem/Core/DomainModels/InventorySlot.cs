using InventorySystem.Data.Items;
using System.Collections.Generic;

namespace InventorySystem.Core.DomainModels
{
	// Represents a single inventory slot (or equip slot) in memory.
	public class InventorySlot
	{
		public string SlotId { get; private set; }
		//public bool IsEquippedSlot { get; private set; }

		public List<BaseItem> EquippedItems { get; private set; }

		public InventorySlot(string slotId)
		{
			SlotId = slotId;
			//IsEquippedSlot = isEquippedSlot;

			EquippedItems = new List<BaseItem>();
		}
	}
}
