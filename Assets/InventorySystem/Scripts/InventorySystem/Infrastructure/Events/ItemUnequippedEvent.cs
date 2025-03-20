using InventorySystem.Data.Enums;

namespace InventorySystem.Infrastructure.Events
{
	public struct ItemUnequippedEvent
	{
		public Data.Items.BaseItem Item;
		public SlotIdEnum SlotId;
	}
}