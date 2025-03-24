using InventorySystem.Core.DomainModels;
using InventorySystem.Data;
using InventorySystem.Data.Enums;
using InventorySystem.Data.Items;
using UnityEngine;

namespace InventorySystem.Infrastructure.Events
{
	public struct OnSubSlotLeftClickedEvent
	{
		public BaseItem SelectedItem { get; set; }
		public int SubSlotId { get; set; }
		public SlotIdEnum SlotId { get; set; }
	}
}