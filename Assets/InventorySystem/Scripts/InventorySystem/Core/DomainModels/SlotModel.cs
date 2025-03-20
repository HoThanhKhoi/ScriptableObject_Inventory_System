using InventorySystem.Data.Enums;
using InventorySystem.Data.Items;
using System.Collections.Generic;

namespace InventorySystem.Core.DomainModels
{
	// Represents a single inventory slot (or equip slot) in memory.
	public class SlotModel
	{
		public SlotIdEnum SlotId { get; private set; }

		public List<BaseItem> EquippedItems { get; private set; }

		public SlotModel(SlotIdEnum slotId)
		{
			SlotId = slotId;
			EquippedItems = new List<BaseItem>();
		}
	}
}
