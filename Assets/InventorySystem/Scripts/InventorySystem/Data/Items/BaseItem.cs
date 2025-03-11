using UnityEngine;
using InventorySystem.Data.Enums;

namespace InventorySystem.Data.Items
{
	[CreateAssetMenu(menuName = "InventorySystem/Items/BaseItem")]
	public class BaseItem : ScriptableObject
	{
		[Header("Basic Info")]
		public string ItemId;
		public string DisplayName;
		public Sprite Icon;

		[Header("Classification")]
		public ItemCategory Category;

		[TextArea]
		public string Description;
	}
}
