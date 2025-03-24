using UnityEngine;
using SuperScrollView;
using VContainer;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Enums;
using System.Linq;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	public class InventoryView : MonoBehaviour
	{
		[SerializeField] private LoopGridView _loopGridView;

		private BaseItem[] _currentItems = new BaseItem[0];
		private IEventBus _eventBus;
		private IInventoryService _inventoryService;


		[Inject]
		public void Construct(IEventBus eventBus, IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
			_eventBus = eventBus;
		}

		private void OnEnable()
		{
			//_eventBus.Subscribe<OnSlotHoveredEvent>(OnSlotHovered);
			//_eventBus.Subscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Subscribe<OnSlotClickedEvent>(OnSlotClickedEvent);
			_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);

		}

		private void OnDisable()
		{
			//_eventBus.Subscribe<OnSlotHoveredEvent>(OnSlotHovered);
			//_eventBus.Subscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Subscribe<OnSlotClickedEvent>(OnSlotClickedEvent);
			_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);
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

		//private void OnSlotHovered(OnSlotHoveredEvent e)
		//{
		//	Debug.Log($"[InventoryView] Slot Hovered item: {e.SlotDefinition?.name}");
		//	if (e.SlotDefinition == null) return;
		//	ShowItemsByCategory(e.SlotDefinition.AllowedCategories[0]);
		//}

		//private void OnSubSlotHovered(OnSubSlotHoveredEvent e)
		//{
		//	Debug.Log($"[InventoryView] Sub Hovered item: {e.SelectedItem?.DisplayName}");
		//	if (e.SelectedItem == null) return;
		//	ShowItemsByCategory(e.SelectedItem.Category);
		//}

		private void OnSlotClickedEvent(OnSlotClickedEvent e)
		{
			Debug.Log($"[InventoryView] Slot Clicked item: {e.SlotDefinition?.name}");
			if (e.SlotDefinition == null) return;
			ShowItemsByCategory(e.SlotDefinition.AllowedCategories[0]);
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
			// "MyInventoryCellUI" must match the prefab name in ItemPrefabList
			LoopGridViewItem itemObj = gridView.NewListViewItem("MyInventoryCellUI");
			var cell = itemObj.GetComponent<MyInventoryCellUI>();
			if (cell != null)
			{
				cell.Init(itemData, _eventBus);
			}

			return itemObj;
		}
	}

}