using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Data.Enums;

namespace InventorySystem.Data
{
	[CreateAssetMenu(menuName = "InventorySystem/SlotDefinition")]
	public class EquipmentSlotDefinition : ScriptableObject
	{
		public string SlotId;
		public List<ItemCategory> AllowedCategories;
	}
}
