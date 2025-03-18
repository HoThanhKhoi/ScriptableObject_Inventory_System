using UnityEngine;
using VContainer;
using InventorySystem.Core.Interfaces;
using InventorySystem.Infrastructure.Events;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Main inventory screen controller:
	// - References big slots & zodiac slots
	// - References left panel
	// - Subscribes to events
	// - Coordinates user interactions

	public class CenterView : MonoBehaviour
	{
		[SerializeField] private SlotView _weaponSlotView;
		[SerializeField] private SlotView _skillGroup1SlotView;
		[SerializeField] private SlotView _skillGroup2SlotView;
		[SerializeField] private SlotView _consumableSlotView;

		private IInventoryService _inventoryService;
		private IEventBus _eventBus;

		[Inject]
		public void Construct(IInventoryService inventoryService, IEventBus eventBus)
		{
			_inventoryService = inventoryService;
			_eventBus = eventBus;
		}

		//private void Start()
		//{
		//	// Subscribe to events if needed
		//	_eventBus.Subscribe<ItemEquippedEvent>(OnItemEquipped);
		//	_eventBus.Subscribe<ItemUnequippedEvent>(OnItemUnequipped);

		//	// Initialize big slots
		//	_weaponSlotView.Init("weaponSlot", OnSlotClicked);
		//	_skillGroup1SlotView.Init("skillGroup1Slot", OnSlotClicked);
		//	_skillGroup2SlotView.Init("skillGroup2Slot", OnSlotClicked);
		//	_consumableSlotView.Init("consumableSlot", OnSlotClicked);

		//	// Initialize zodiac slots (if you have them)
		//	if (_zodiacPanelView != null)
		//	{
		//		_zodiacPanelView.Init(OnSlotClicked);
		//	}

		//	// Show all items by default in the left panel
		//	var allItems = _inventoryService.GetAllItems();
		//	_leftPanelView.ShowItems(allItems);
		//}

		//private void OnDestroy()
		//{
		//	_eventBus.Unsubscribe<ItemEquippedEvent>(OnItemEquipped);
		//	_eventBus.Unsubscribe<ItemUnequippedEvent>(OnItemUnequipped);
		//}

		//private void OnSlotClicked(string slotId)
		//{
		//	// Filter items for that slot if desired
		//	// For now, we just show all items again or do something advanced
		//	var allItems = _inventoryService.GetAllItems();
		//	_leftPanelView.ShowItems(allItems);
		//}

		//private void OnItemEquipped(ItemEquippedEvent evt)
		//{
		//	// Could refresh specific slot view if needed
		//	if (evt.SlotId == "weaponSlot")
		//	{
		//		_weaponSlotView.SetEquippedItem(evt.Item);
		//	}
		//	// etc. or do a generic approach
		//}

		//private void OnItemUnequipped(ItemUnequippedEvent evt)
		//{
		//	// Refresh slot if needed
		//	if (evt.SlotId == "weaponSlot")
		//	{
		//		_weaponSlotView.SetEquippedItem(null);
		//	}
		//}
	}
}
