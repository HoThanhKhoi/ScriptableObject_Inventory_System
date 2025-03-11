namespace InventorySystem.Infrastructure.Events
{
	public struct ItemUnequippedEvent
	{
		public Data.Items.BaseItem Item;
		public string SlotId;
	}
}