using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Data.Enums;

namespace InventorySystem.Data
{
	[CreateAssetMenu(menuName = "InventorySystem/SlotDefinition")]
	public class EquipmentSlotDefinition : ScriptableObject
	{
		[SerializeField] private SlotIdEnum _slotId;
		[SerializeField] private List<ItemCategoryEnum> _allowedCategories;
		[SerializeField] private int _capacity;

		public SlotIdEnum SlotId => _slotId;
		public List<ItemCategoryEnum> AllowedCategories => _allowedCategories;
		public int Capacity => _capacity;

		public EquipmentSlotDefinition Clone()
		{
			EquipmentSlotDefinition clone = CreateInstance<EquipmentSlotDefinition>();
			clone._slotId = _slotId;
			clone._allowedCategories = new List<ItemCategoryEnum>(_allowedCategories);
			clone._capacity = _capacity;
			return clone;
		}
	}
}
