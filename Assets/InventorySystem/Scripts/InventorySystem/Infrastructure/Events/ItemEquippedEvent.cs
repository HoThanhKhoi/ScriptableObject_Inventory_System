using InventorySystem.Core.DomainModels;
using InventorySystem.Data.Enums;

namespace InventorySystem.Infrastructure.Events
{
	public struct ItemEquippedEvent
	{
		public SubSlotModel SubSlotModel { get; set; }
	}
}