using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using InventorySystem.Data.Items;
using TMPro;
using UnityEngine.EventSystems;
using InventorySystem.Infrastructure.Events;
using VContainer;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Enums;
using static UnityEditor.Progress;

public class MyInventoryCellUI : LoopGridViewItem
{
	[SerializeField] private TextMeshProUGUI _itemNameText;
	[SerializeField] private Image _itemIcon;
	[SerializeField] private Button _cellButton;

	private BaseItem _boundItem;
	private IEventBus _eventBus;
	private IInventoryService _inventoryService;
	private SlotIdEnum _currentSlotId;
	private int _currentSubSlotId;

	private void Awake()
	{

	}

	private void OnEnable()
	{
		_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(SetSubSlotLeftClicked);
	}

	private void OnDisable()
	{
		_eventBus.Unsubscribe<OnSubSlotLeftClickedEvent>(SetSubSlotLeftClicked);
	}

	public void Init(BaseItem item, IEventBus eventBus)
	{
		_eventBus = eventBus;
		SetItemData(item);
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

	public void OnPointerClickEvent(BaseEventData baseEvent)
	{
		// Attempt to cast BaseEventData -> PointerEventData
		PointerEventData eventData = baseEvent as PointerEventData;
		if (eventData == null)
			return;

		// Get the clicked item

		// Now check which mouse button was used
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			HandleItemLeftClick(_boundItem);
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			HandleItemRightClick(_boundItem);
		}
	}

	public void HandleItemRightClick(BaseItem clickedItem)
	{
		Debug.Log("HandleItemRightClick");
		//_eventBus.Publish(new OnSubSlotRightClickedEvent { SelectedItem = _equippedItem, SlotDefinition = _equipmentSlotDefinition });
	}

	public void HandleItemLeftClick(BaseItem clickedItem)
	{
		bool isSubSlotClicked = _inventoryService.GetSubSlotClickedStatus();

		if (isSubSlotClicked)
		{
			_inventoryService.EquipItem(clickedItem, _currentSlotId, _currentSubSlotId);

			Debug.Log($"Item equipped: {clickedItem.DisplayName}");

			_eventBus.Publish(new ItemSelectedEvent { SelectedItem = clickedItem });
			return;
		}
		if (!isSubSlotClicked)
		{
			Debug.Log("Sub Slot is not clicked, return");
			return;
		}

		//_inventoryService.EquipItem(clickedItem);

		//_eventBus.Publish(new OnSubSlotLeftClickedEvent { SelectedItem = _equippedItem, SlotDefinition = _equipmentSlotDefinition });
	}

	private void SetSubSlotLeftClicked(OnSubSlotLeftClickedEvent e)
	{
		_currentSlotId = e.SlotId;
		_currentSubSlotId = e.SubSlotId;
	}
}
