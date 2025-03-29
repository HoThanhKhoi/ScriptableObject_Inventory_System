using UnityEngine;
using UnityEngine.UI;
using InventorySystem.Data.Items;
using InventorySystem.Data;
using InventorySystem.Infrastructure.Events;
using VContainer;
using InventorySystem.Data.Enums;
using InventorySystem.Core.DomainModels;
using UnityEngine.EventSystems;
using InventorySystem.Core.Interfaces;
using TMPro;
using System;
using Unity.VisualScripting;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Represents one equip slot in the UI (weapon, consumable, zodiac, etc.).
	public class SubSlotView : MonoBehaviour
	{
		[SerializeField] private Button _slotButton;
		[SerializeField] private Image _iconImage;
		[SerializeField] private TextMeshProUGUI _subSlotText;

		[SerializeField] private string _defaultText;
		[SerializeField] private Sprite _defaultIconImage;

		private BaseItem _equippedItem;
		private IEventBus _eventBus;
		private IInventoryService _inventoryService;

		public SubSlotModel SubSlotModel { get; private set; }
		public int SubSlotId { get; private set; }
		public SlotIdEnum SlotId { get; private set; }

		[Inject]
		public void Construct(IEventBus eventBus, IInventoryService inventoryService)
		{
			Debug.Log("Construct SubSlotView is running");

			if (eventBus != null)
			{
				_eventBus = eventBus;
			}

			if (inventoryService != null)
			{
				_inventoryService = inventoryService;

			}
		}

		private void OnEnable()
		{

		}

		private void OnDisable()
		{

		}

		private void Awake()
		{
			VContainerUtils.AutoInjectSelf(this);
		}

		private void Start()
		{

		}

		public void HandleSubSlotHovered()
		{
			_eventBus.Publish(new OnSubSlotHoveredEvent { SelectedItem = _equippedItem });
		}

		public void OnPointerClickEvent(BaseEventData baseEvent)
		{
			// Attempt to cast BaseEventData -> PointerEventData
			PointerEventData eventData = baseEvent as PointerEventData;
			if (eventData == null)
				return;

			// Now check which mouse button was used
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				HandleSubSlotLeftClick();
			}
			else if (eventData.button == PointerEventData.InputButton.Right)
			{
				HandleSubSlotRightClick();
			}
		}

		public void HandleSubSlotRightClick()
		{
			Debug.Log($"[SubSlotView] Right Clicked item: {SubSlotId}, {SlotId}");
			_eventBus.Publish(new OnSubSlotRightClickedEvent { SelectedItem = _equippedItem, SlotId = SlotId, SubSlotId = SubSlotId });
		}

		public void HandleSubSlotLeftClick()
		{
			Debug.Log($"[SubSlotView] Left Clicked item: {SubSlotId}, {SlotId}");
			Debug.Log($"{_inventoryService == null}");
			_eventBus.Publish(new OnSubSlotLeftClickedEvent { SelectedItem = _equippedItem, SlotId = SlotId, SubSlotId = SubSlotId });
		}

		public void SetupSubSlot(int subSlotId, SlotIdEnum slotId)
		{
			SubSlotId = subSlotId;
			SlotId = slotId;
		}

		public void UpdateSubSlotItemUI(SubSlotModel subSlotModel)
		{
			_inventoryService.SetSubSlotClickedStatus(false);

			_equippedItem = subSlotModel.EquippedItem;

			if (_iconImage == null) return;
			if (_subSlotText == null) return;

			if (_equippedItem == null)
			{
				_subSlotText.enabled = true;
				_subSlotText.text = _defaultText;
				_iconImage.enabled = true;
				_iconImage.sprite = _defaultIconImage;
			}
			else
			{
				_subSlotText.enabled = true;
				_subSlotText.text = _equippedItem.name;
				_iconImage.enabled = true;
				_iconImage.sprite = _equippedItem.Icon;
			}

			Debug.Log($"[SubSlotView] Item equipped: {subSlotModel == null}, {subSlotModel.EquippedItem == null}, {SlotId == null}");
		}



		////private void HandleSlotHovered()
		//{
		//	//_eventBus.Publish(new OnSlotHoveredEvent { SelectedItem = clickedItem });
		//}

		//public void Init(string slotId, System.Action<string> onSlotClicked)
		//{
		//	_slotId = slotId;
		//	_onSlotClicked = onSlotClicked;
		//	if (_slotButton != null)
		//	{
		//		_slotButton.OnPointerEnter.AddListener(HandleSlotClick);
		//	}
		//}

		//public void SetEquippedItem(BaseItem item)
		//{
		//	_equippedItem = item;
		//	if (_iconImage == null) return;

		//	if (item == null)
		//	{
		//		_iconImage.enabled = false;
		//	}
		//	else
		//	{
		//		_iconImage.enabled = true;
		//		_iconImage.sprite = item.Icon;
		//	}
		//}

	}
}
