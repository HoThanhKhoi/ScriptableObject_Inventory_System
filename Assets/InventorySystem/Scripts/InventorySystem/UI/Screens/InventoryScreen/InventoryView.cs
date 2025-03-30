using UnityEngine;
using SuperScrollView;
using VContainer;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Enums;
using System.Linq;
using InventorySystem.Data;
using System;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	public class InventoryView : MonoBehaviour
	{
		[SerializeField] private LoopGridView _loopGridView;

		private BaseItem[] _currentItems = new BaseItem[0];
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
			//_eventBus.Subscribe<OnSlotHoveredEvent>(OnSlotHovered);
			//_eventBus.Subscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Subscribe<OnSlotClickedEvent>(OnSlotClickedEvent);

			_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);
			_eventBus.Subscribe<OnSubSlotRightClickedEvent>(OnSubSlotRightClicked);

			_eventBus.Subscribe<ItemEquippedEvent>(OnItemEquipped);
			_eventBus.Subscribe<ItemUnequippedEvent>(OnItemUnequipped);
			_eventBus.Subscribe<ItemSelectedEvent>(OnItemSelected);

		}

		private void OnDisable()
		{
			//_eventBus.Subscribe<OnSlotHoveredEvent>(OnSlotHovered);
			//_eventBus.Subscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Unsubscribe<OnSlotClickedEvent>(OnSlotClickedEvent);

			_eventBus.Unsubscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);
			_eventBus.Unsubscribe<OnSubSlotRightClickedEvent>(OnSubSlotRightClicked);

			_eventBus.Unsubscribe<ItemEquippedEvent>(OnItemEquipped);
			_eventBus.Unsubscribe<ItemUnequippedEvent>(OnItemUnequipped);
			_eventBus.Unsubscribe<ItemSelectedEvent>(OnItemSelected);
		}

		

		private void Start()
		{
			// Initialize the grid with the current item count
			if (_loopGridView != null)
			{
				_loopGridView.InitGridView(_currentItems.Length, OnGetItemByRowColumn);
			}
		}

		// Equip the item
		private void OnItemSelected(ItemSelectedEvent e)
		{
			SlotIdEnum _currentSlotId = _inventoryService.GetCurrentSlotId();
			int _currentSubSlotId = _inventoryService.GetCurrentSubSlotId();
			bool isSubSlotClicked = _inventoryService.GetSubSlotClickedStatus();

			if (isSubSlotClicked)
			{
				_inventoryService.EquipItem(e.SelectedItem, _currentSlotId, _currentSubSlotId);
				return;
			}
			if (!isSubSlotClicked)
			{
				return;
			}
		}

		// Unequip the item
		private void OnSubSlotRightClicked(OnSubSlotRightClickedEvent e)
		{
			_inventoryService.UnequipItem(e.SlotId, e.SubSlotId);
		}

		// Show items by category
		private void OnSlotClickedEvent(OnSlotClickedEvent e)
		{
			if (e.SlotDefinition == null) return;

			EquipmentSlotDefinition equipmentSlotDefinition = _equipmentService.GetSlotDefinitionByIdFromList(e.SlotDefinition.SlotId);

			ShowItemsByCategory(equipmentSlotDefinition.AllowedCategories[0]);
		}

		// Show items by category
		private void OnSubSlotLeftClicked(OnSubSlotLeftClickedEvent e)
		{
			if (e.SelectedItem == null) return;
			ShowItemsByCategory(e.SelectedItem.Category);
		}

		private void OnItemEquipped(ItemEquippedEvent e)
		{
			RefreshEquippedIndication();
		}

		private void OnItemUnequipped(ItemUnequippedEvent e)
		{
			RefreshEquippedIndication();
		}

		private void RefreshEquippedIndication()
		{
			// Iterate over all current items. Only visible items will be updated.
			for (int i = 0; i < _currentItems.Length; i++)
			{
				// Get the cell if it is currently visible.
				LoopGridViewItem gridItem = _loopGridView.GetShownItemByItemIndex(i);
				if (gridItem != null)
				{
					ItemView cell = gridItem.GetComponent<ItemView>();
					if (cell != null)
					{
						// Query the inventory service to check if the item bound to this cell is equipped.
						bool isEquipped = _inventoryService.IsItemEquipped(cell.BoundItem);
						cell.ShowAndHideEquippedIndicator(isEquipped);
					}
				}
			}
		}

		private void ShowItemsByCategory (ItemCategoryEnum category)
		{
			var allItems = _inventoryService.GetAllItems();

			var filtered = allItems.Where(item => item.Category == category).ToArray();

			_currentItems = filtered;

			if (_loopGridView == null) return;

			// Update the item count and refresh
			_loopGridView.SetListItemCount(_currentItems.Length, false);
			_loopGridView.RefreshAllShownItem();
		}

		private LoopGridViewItem OnGetItemByRowColumn(LoopGridView gridView, int index, int row, int column)
		{
			if (index < 0 || index >= _currentItems.Length)
				return null;

			var itemData = _currentItems[index];

			// Create or reuse a cell from the pool
			// "ItemButton" must match the prefab name in ItemPrefabList
			LoopGridViewItem itemObj = gridView.NewListViewItem("ItemButton");
			ItemView cell = itemObj.GetComponent<ItemView>();
			if (cell != null)
			{
				cell.Init(itemData, _eventBus);

				bool isEquipped = _inventoryService.IsItemEquipped(itemData);
				cell.ShowAndHideEquippedIndicator(isEquipped);
			}

			Debug.Log($"[InventoryView] OnGetItemByRowColumn: index: {index}, row: {row}, column: {column}");

			return itemObj;
		}

		private int GetEquippedItemIndex(BaseItem item)
		{
			return Array.IndexOf(_currentItems, item);
		}
	}
}