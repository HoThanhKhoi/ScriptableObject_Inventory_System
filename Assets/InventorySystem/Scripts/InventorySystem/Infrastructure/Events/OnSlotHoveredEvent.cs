using InventorySystem.Data.Items;
using UnityEngine;

namespace InventorySystem.Infrastructure.Events
{
	public struct OnSlotHoveredEvent
	{
		public BaseItem SelectedItem { get; set; }
	}
}

