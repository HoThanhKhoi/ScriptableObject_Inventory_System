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

public class ItemView : LoopGridViewItem
{
	[SerializeField] private TextMeshProUGUI _itemNameText;
	[SerializeField] private Image _itemIcon;
	[SerializeField] private Button _cellButton;

	private BaseItem _boundItem;
	private IEventBus _eventBus;
	private IInventoryService _inventoryService;

	[Inject]
	public void Construct(IEventBus eventBus, IInventoryService inventoryService)
	{
		if (eventBus == null) return;
		_eventBus = eventBus;
		_inventoryService = inventoryService;
	}



	private void Awake()
	{
		VContainerUtils.AutoInjectSelf(this);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
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

	public void OnPointerEnterEvent(BaseEventData baseEvent)
	{
		_eventBus.Publish(new ItemHoveredEvent { SelectedItem = _boundItem });
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
		//_eventBus.Publish(new OnSubSlotRightClickedEvent { SelectedItem = _equippedItem, SlotDefinition = _equipmentSlotDefinition });
	}

	public void HandleItemLeftClick(BaseItem clickedItem)
	{
		_eventBus.Publish(new ItemSelectedEvent { SelectedItem = clickedItem });
	}
}
