using InventorySystem.Core.DomainModels;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Enums;
using InventorySystem.Infrastructure.Events;
using SuperScrollView;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	public class EquipmentSubSlotView : MonoBehaviour
	{
		[SerializeField] private int columnCount = 5;
		[SerializeField] GameObject subSlotButton;

		private Vector2 _currentPosition;

		Dictionary<int, SubSlotModel> subSlotButtons = new Dictionary<int, SubSlotModel>();

		private List<SubSlotModel> _subSlots = new List<SubSlotModel>();

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
		}

		private void OnItemSelected(OnSlotClickedEvent e)
		{
			SlotIdEnum slotId = _equipmentService.GetSlotId(e.SlotDefinition);

			Debug.Log($"[EquipmentSubSlotView] Clicked item: {e.SlotDefinition.AllowedCategories[0]}, {e.SlotDefinition.SlotId}, {e.SlotDefinition.Capacity}");

			SlotModel slot = _inventoryService.GetSlot(slotId);



			Debug.Log($"[EquipmentSubSlotView] Clicked item: {slot == null}");
		}
	}

}
