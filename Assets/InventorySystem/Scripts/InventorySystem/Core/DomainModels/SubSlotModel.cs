using InventorySystem.Data.Enums;
using InventorySystem.Data.Items;

namespace InventorySystem.Core.DomainModels
{
	public class SubSlotModel
	{
		public int SubSlotId;
		public SlotIdEnum SlotId;
		public BaseItem EquippedItem;

		public SubSlotModel(int subSlotId, BaseItem equippedItem)
		{
			SubSlotId = subSlotId;
			EquippedItem = equippedItem;
		}
	}

}
