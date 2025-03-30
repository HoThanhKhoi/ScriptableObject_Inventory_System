using InventorySystem.Core.DomainModels;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data;
using InventorySystem.Data.Enums;
using InventorySystem.Infrastructure.Events;
using SuperScrollView;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using static UnityEditor.Experimental.GraphView.Port;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	public class EquipmentSubSlotView : MonoBehaviour
	{
		[SerializeField] private int columnCount = 5;
		[SerializeField] GameObject subSlotButton;
		[SerializeField] private int subSlotButtonPadding;
		[SerializeField] private Vector2 _initialSubSlotPosition;

		private Vector2 _currentSubSlotPosition;
		private RectTransform _rectTransform;
		private int slotCapacity;
		private int maxCapacity = 10;

		Dictionary<int, SubSlotView> subSlotButtons = new Dictionary<int, SubSlotView>();

		private SlotModel slotModel;

		private IEventBus _eventBus;
		private IInventoryService _inventoryService;
		private IEquipmentSlotService _equipmentService;

		[Inject]
		public void Construct(IEventBus eventBus, IInventoryService inventoryService, IEquipmentSlotService equipmentService)
		{
			_inventoryService = inventoryService;
			_equipmentService = equipmentService;
			_eventBus = eventBus;
		}

		private void OnEnable()
		{
			_eventBus.Subscribe<OnSlotClickedEvent>(OnSlotClicked);

			_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);

			_eventBus.Subscribe<ItemUnequippedEvent>(OnItemUnequipped);
			_eventBus.Subscribe<ItemEquippedEvent>(OnItemEquipped);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe<OnSlotClickedEvent>(OnSlotClicked);

			_eventBus.Unsubscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);

			_eventBus.Unsubscribe<ItemUnequippedEvent>(OnItemUnequipped);
			_eventBus.Unsubscribe<ItemEquippedEvent>(OnItemEquipped);
		}

		private void Start()
		{
			_currentSubSlotPosition = _initialSubSlotPosition;
			CreateSubSlotButtons(maxCapacity);
		}
		
		private void OnItemUnequipped(ItemUnequippedEvent e)
		{
			SubSlotModel subSlotModel = _inventoryService.GetSubSlot(e.SlotId, e.SubSlotId);
			SubSlotView subSlotView = subSlotButtons[e.SubSlotId];
			subSlotView.UpdateSubSlotItemUI(subSlotModel);

			Debug.Log($"[EquipmentSubSlotView] Item unequipped: {e.Item?.name}, {e.SlotId}, {e.SubSlotId}");
			Debug.Log($"[EquipmentSubSlotView] Item unequipped:  {subSlotModel?.SlotId}, {subSlotModel?.SubSlotId}, {subSlotModel?.EquippedItem}");
		}

		private void OnSubSlotLeftClicked(OnSubSlotLeftClickedEvent e)
		{
			_inventoryService.SetSubSlotClickedStatus(true);
			_inventoryService.SetCurrentSubSlotId(e.SubSlotId);
		}
		
		private void OnSlotClicked(OnSlotClickedEvent e)
		{
			//Debug.Log($"[EquipmentSubSlotView] Clicked item: " +
			//	$"{e.SlotDefinition.AllowedCategories[0]}, {e.SlotDefinition.SlotId}, {e.SlotDefinition.Capacity}");

			slotModel = _inventoryService.GetSlot(e.SlotDefinition.SlotId);

			slotCapacity = _equipmentService.GetCapacity(e.SlotDefinition.SlotId);

			SubSlotView subSlotView;

			for (int i = 0; i < slotCapacity; i++)
			{
				subSlotView = subSlotButtons[i];

				//Debug.Log($"[EquipmentSubSlotView] Clicked item: {slotModel == null}, {slotModel.SlotId == null}, {subSlotView.SlotId == null}");
				subSlotView.SetupSubSlot(i, slotModel.SlotId);

				SubSlotModel subSlotModel = _inventoryService.GetSubSlot(slotModel.SlotId, i);
				subSlotView.UpdateSubSlotItemUI(subSlotModel);

				subSlotView.gameObject.SetActive(true);
			}

			for (int i = slotCapacity; i < maxCapacity; i++)
			{
				subSlotView = subSlotButtons[i];
				subSlotView.gameObject.SetActive(false);
			}
		}

		private void OnItemEquipped(ItemEquippedEvent e)
		{
			SubSlotModel subSlotModel = _inventoryService.GetSubSlot(e.SubSlotModel.SlotId, e.SubSlotModel.SubSlotId);

			SubSlotView subSlotView = subSlotButtons[e.SubSlotModel.SubSlotId];

			subSlotView.UpdateSubSlotItemUI(subSlotModel);
		}

		private void CreateSubSlotButtons(int maxCapacity)
		{
			for (int i = 0; i < maxCapacity; i++)
			{
				GameObject button = Instantiate(subSlotButton, transform);
				
				_rectTransform = button.GetComponent<RectTransform>();
				_rectTransform.anchoredPosition = _currentSubSlotPosition;
				
				button.SetActive(false);

				SubSlotView subSlotView = button.GetComponent<SubSlotView>();
				subSlotButtons.Add(i, subSlotView);

				_currentSubSlotPosition.x += subSlotButtonPadding;
				if (i == columnCount - 1)
				{
					_currentSubSlotPosition.y -= subSlotButtonPadding;
					_currentSubSlotPosition.x = _initialSubSlotPosition.x;
				}

				//VContainerUtils.InjectGameObject(subSlotView.gameObject);
			}
		}
	}
}
