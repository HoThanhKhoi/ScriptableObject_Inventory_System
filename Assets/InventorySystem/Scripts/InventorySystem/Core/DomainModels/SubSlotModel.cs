using InventorySystem.Data.Enums;
using InventorySystem.Data.Items;

namespace InventorySystem.Core.DomainModels
{
	public class SubSlotModel
	{
		public int SubSlotId;
		public SlotIdEnum SlotId;
		public BaseItem EquippedItem;

		public SubSlotModel(SlotIdEnum slotId, int subSlotId, BaseItem equippedItem)
		{
			SlotId = slotId;
			SubSlotId = subSlotId;
			EquippedItem = equippedItem;
		}
	}
}
