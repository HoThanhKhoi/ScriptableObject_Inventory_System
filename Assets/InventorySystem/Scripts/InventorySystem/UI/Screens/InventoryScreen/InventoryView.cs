using UnityEngine;
using SuperScrollView;
using VContainer;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Enums;
using System.Linq;
using InventorySystem.Data;

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
			_eventBus.Subscribe<ItemSelectedEvent>(OnItemSelected);

		}

		private void OnDisable()
		{
			//_eventBus.Subscribe<OnSlotHoveredEvent>(OnSlotHovered);
			//_eventBus.Subscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Unsubscribe<OnSlotClickedEvent>(OnSlotClickedEvent);
			_eventBus.Unsubscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);
			_eventBus.Unsubscribe<ItemSelectedEvent>(OnItemSelected);
		}

		private void Start()
		{
			// Initialize the grid with the current item count
			if (_loopGridView != null)
			{
				// itemTotalCount = _currentItems.Length
				// onGetItemByRowColumn = OnGetItemByRowColumn
				_loopGridView.InitGridView(_currentItems.Length, OnGetItemByRowColumn);
			}
		}

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
				Debug.Log("Sub Slot is not clicked, return");
				return;
			}
		}

		private void OnSlotClickedEvent(OnSlotClickedEvent e)
		{
			Debug.Log($"[InventoryView] Slot Clicked item: {e.SlotDefinition?.name}");
			if (e.SlotDefinition == null) return;

			EquipmentSlotDefinition equipmentSlotDefinition = _equipmentService.GetSlotDefinitionByIdFromList(e.SlotDefinition.SlotId);

			ShowItemsByCategory(equipmentSlotDefinition.AllowedCategories[0]);
		}

		private void OnSubSlotLeftClicked(OnSubSlotLeftClickedEvent e)
		{
			Debug.Log($"[InventoryView] Sub Left Clicked item: {e.SelectedItem?.DisplayName}");
			if (e.SelectedItem == null) return;
			ShowItemsByCategory(e.SelectedItem.Category);
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
			var cell = itemObj.GetComponent<ItemView>();
			if (cell != null)
			{
				cell.Init(itemData, _eventBus);
			}

			return itemObj;
		}
	}

}