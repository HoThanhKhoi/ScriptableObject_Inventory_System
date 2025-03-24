using InventorySystem.Data.Items;
using UnityEngine;

namespace InventorySystem.Infrastructure.Events
{
	public struct OnSubSlotHoveredEvent
	{
		public BaseItem SelectedItem { get; set; }
	}
}