using InventorySystem.Data.Items;

namespace InventorySystem.Infrastructure.Events
{
	public struct ItemAddedEvent
	{
		public Data.Items.BaseItem Item;
	}
}