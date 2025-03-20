using InventorySystem.Data.Enums;

namespace InventorySystem.Infrastructure.Events
{
	public struct ItemEquippedEvent
	{
		public Data.Items.BaseItem Item;
		public SlotIdEnum SlotId;
	}
}