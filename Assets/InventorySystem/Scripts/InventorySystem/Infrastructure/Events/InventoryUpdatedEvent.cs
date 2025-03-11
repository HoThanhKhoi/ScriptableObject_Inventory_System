namespace InventorySystem.Infrastructure.Events
{
	public struct InventoryUpdatedEvent
	{
		// Could be triggered when any major change occurs (add, remove, equip, etc.)
		// Potentially store a reference or snapshot of the entire inventory if needed.
	}
}