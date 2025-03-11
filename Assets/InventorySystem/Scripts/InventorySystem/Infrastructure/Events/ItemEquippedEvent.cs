namespace InventorySystem.Infrastructure.Events
{
	public struct ItemEquippedEvent
	{
		public Data.Items.BaseItem Item;
		public string SlotId;
	}
}