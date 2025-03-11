using UnityEngine;
using UnityEngine.UI;
using InventorySystem.Data.Items;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Represents one equip slot in the UI (weapon, consumable, zodiac, etc.).
	public class InventorySlotView : MonoBehaviour
	{
		[SerializeField] private Image _iconImage;
		[SerializeField] private Button _slotButton;

		private string _slotId;
		private System.Action<string> _onSlotClicked;

		private BaseItem _equippedItem;

		public void Init(string slotId, System.Action<string> onSlotClicked)
		{
			_slotId = slotId;
			_onSlotClicked = onSlotClicked;
			if (_slotButton != null)
			{
				_slotButton.onClick.AddListener(HandleSlotClick);
			}
		}

		public void SetEquippedItem(BaseItem item)
		{
			_equippedItem = item;
			if (_iconImage == null) return;

			if (item == null)
			{
				_iconImage.enabled = false;
			}
			else
			{
				_iconImage.enabled = true;
				_iconImage.sprite = item.Icon;
			}
		}

		private void HandleSlotClick()
		{
			_onSlotClicked?.Invoke(_slotId);
		}
	}
}
