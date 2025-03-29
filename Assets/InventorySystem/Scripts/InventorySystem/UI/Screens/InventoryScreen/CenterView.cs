using UnityEngine;
using VContainer;
using InventorySystem.Core.Interfaces;
using InventorySystem.Infrastructure.Events;
using System;
using InventorySystem.Data.Enums;
using InventorySystem.Data;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Main inventory screen controller:
	// - References big slots & zodiac slots
	// - References left panel
	// - Subscribes to events
	// - Coordinates user interactions

	public class CenterView : MonoBehaviour
	{
		private IInventoryService _inventoryService;
		private IEventBus _eventBus;

		[Inject]
		public void Construct(IInventoryService inventoryService, IEventBus eventBus)
		{
			_inventoryService = inventoryService;
			_eventBus = eventBus;
		}

		private void OnEnable()
		{
			_eventBus.Subscribe<OnSlotClickedEvent>(OnSlotClicked); // <--- Add this line here>
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe<OnSlotClickedEvent>(OnSlotClicked); // <--- Add this line here>
		}

		private void OnSlotClicked(OnSlotClickedEvent e)
		{
			_inventoryService.SetCurrentSlotId(e.SlotDefinition.SlotId);
		}
	}
}
