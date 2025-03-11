using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using InventorySystem.Data.Items;
using TMPro;

public class MyInventoryCellUI : LoopListViewItem2
{
	[SerializeField] private TextMeshProUGUI _itemNameText;
	[SerializeField] private Image _itemIcon;
	[SerializeField] private Button _cellButton;

	private BaseItem _boundItem;

	// This is the callback the parent sets
	public System.Action<BaseItem> OnCellClicked;

	private void Awake()
	{
		if (_cellButton != null)
		{
			_cellButton.onClick.AddListener(HandleClick);
		}
	}

	public void SetItemData(BaseItem item)
	{
		_boundItem = item;
		if (_itemNameText != null) _itemNameText.text = item != null ? item.DisplayName : "Unknown";
		if (_itemIcon != null)
		{
			_itemIcon.enabled = item != null && item.Icon != null;
			if (item?.Icon != null)
				_itemIcon.sprite = item.Icon;
		}
	}

	private void HandleClick()
	{
		OnCellClicked?.Invoke(_boundItem);
	}
}
