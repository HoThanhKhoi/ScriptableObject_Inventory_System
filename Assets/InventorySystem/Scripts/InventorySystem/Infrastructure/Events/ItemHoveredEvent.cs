using InventorySystem.Data;
using InventorySystem.Data.Items;
using UnityEngine;

namespace InventorySystem.Infrastructure.Events
{
	public struct ItemHoveredEvent
	{
		public BaseItem SelectedItem { get; set; }
	}
}

