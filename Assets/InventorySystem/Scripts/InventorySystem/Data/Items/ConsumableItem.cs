using UnityEngine;

namespace InventorySystem.Data.Items
{
	[CreateAssetMenu(menuName = "InventorySystem/Items/ConsumableItem")]
	public class ConsumableItem : BaseItem
	{
		[Header("Consumable Stats")]
		public float HealingAmount;
		public float StaminaRecoveryAmount;
	}
}
