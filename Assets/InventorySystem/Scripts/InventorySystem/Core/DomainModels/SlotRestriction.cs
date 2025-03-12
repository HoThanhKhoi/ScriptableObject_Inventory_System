using InventorySystem.Data.Enums;
using System.Collections.Generic;

namespace InventorySystem.Core.DomainModels
{
	// Holds which categories are allowed for a specific slot.
	public class SlotRestriction
	{
		public string SlotId { get; private set; }
		public List<ItemCategory> AllowedCategories { get; private set; }
		public int Capacity { get; }

		public SlotRestriction(string slotId, List<ItemCategory> allowedCategories, int capacity)
		{
			SlotId = slotId;
			AllowedCategories = allowedCategories;
			Capacity = capacity;
		}
	}
}
