using InventorySystem.Data;
using InventorySystem.Data.Items;
using UnityEngine;

namespace InventorySystem.Infrastructure.Events
{
	public struct OnSlotClickedEvent
	{
		public BaseItem SelectedItem { get; set; }
		public EquipmentSlotDefinition SlotDefinition { get; set; }
	}
}