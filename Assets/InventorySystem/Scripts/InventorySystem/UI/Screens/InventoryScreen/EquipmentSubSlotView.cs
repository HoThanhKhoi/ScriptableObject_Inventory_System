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
		[SerializeField] private LoopGridView _loopGridView;

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

			Debug.Log(slotId);
			Debug.Log($"[EquipmentSubSlotView] Clicked item: {e.SelectedItem?.DisplayName}");
		}
	}

}
