using InventorySystem.Core.DomainModels;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data;
using InventorySystem.Data.Enums;
using InventorySystem.Infrastructure.Events;
using SuperScrollView;
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
		private List<SubSlotModel> subSlotModels = new List<SubSlotModel>();

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
			_eventBus.Subscribe<OnSlotClickedEvent>(OnItemSelected);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe<OnSlotClickedEvent>(OnItemSelected);
		}

		private void Start()
		{
			_currentSubSlotPosition = _initialSubSlotPosition;
			CreateSubSlots(maxCapacity);
		}

		private void OnItemSelected(OnSlotClickedEvent e)
		{
			EquipmentSlotDefinition slotDefinition = e.SlotDefinition;

			slotCapacity = _equipmentService.GetCapacity(slotDefinition);

			SlotIdEnum slotId = _equipmentService.GetSlotId(e.SlotDefinition);

			Debug.Log($"[EquipmentSubSlotView] Clicked item: {e.SlotDefinition.AllowedCategories[0]}, {e.SlotDefinition.SlotId}, {e.SlotDefinition.Capacity}");

			slotModel = _inventoryService.GetSlot(slotId);

			SubSlotView subSlot;

			for (int i = 0; i < slotCapacity; i++)
			{
				subSlot = subSlotButtons[i];
				subSlot.gameObject.SetActive(true);
			}

			for (int i = slotCapacity; i < maxCapacity; i++)
			{
				subSlot = subSlotButtons[i];
				subSlot.gameObject.SetActive(false);
			}

			Debug.Log($"[EquipmentSubSlotView] Clicked item: {slotModel == null}");
		}

		private void CreateSubSlots(int maxCapacity)
		{
			for (int i = 0; i < maxCapacity; i++)
			{
				GameObject button = Instantiate(subSlotButton, transform);
				
				_rectTransform = button.GetComponent<RectTransform>();
				_rectTransform.anchoredPosition = _currentSubSlotPosition;
				
				button.SetActive(false);
				
				SubSlotView subSlot = button.GetComponent<SubSlotView>();
				subSlotButtons.Add(i, subSlot);
				
				_currentSubSlotPosition.x += subSlotButtonPadding;
				if (i == columnCount - 1)
				{
					_currentSubSlotPosition.y -= subSlotButtonPadding;
					_currentSubSlotPosition.x = _initialSubSlotPosition.x;
				}
			}
		}

	}

}
