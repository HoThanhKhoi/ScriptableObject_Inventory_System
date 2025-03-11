using UnityEngine;

namespace InventorySystem.Data.Items
{
	[CreateAssetMenu(menuName = "InventorySystem/Items/WeaponItem")]
	public class WeaponItem : BaseItem
	{
		[Header("Weapon Stats")]
		public float AttackPower;
		public float AttackSpeed;
		// Add more fields as needed (e.g. range, stamina cost, etc.)
	}
}
