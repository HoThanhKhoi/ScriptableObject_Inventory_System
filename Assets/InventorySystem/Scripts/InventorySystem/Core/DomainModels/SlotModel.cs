using InventorySystem.Data.Items;
using System.Collections.Generic;

namespace InventorySystem.Core.DomainModels
{
	// Represents a single inventory slot (or equip slot) in memory.
	public class SlotModel
	{
		public string SlotId { get; private set; }

		public List<BaseItem> EquippedItems { get; private set; }

		public SlotModel(string slotId)
		{
			SlotId = slotId;
			EquippedItems = new List<BaseItem>();
		}
	}
}
