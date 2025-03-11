using UnityEngine;
using SuperScrollView;
using VContainer;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;

public class InventoryLeftPanelView : MonoBehaviour
{
	[SerializeField] private LoopGridView _loopGridView;

	private BaseItem[] _currentItems = new BaseItem[0];
	private IEventBus _eventBus;

	[Inject]
	public void Construct(IEventBus eventBus)
	{
		_eventBus = eventBus;
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

	public void ShowItems(BaseItem[] items)
	{
		_currentItems = items ?? new BaseItem[0];
		if (_loopGridView == null) return;

		// Update the item count and refresh
		_loopGridView.SetListItemCount(_currentItems.Length, false);
		_loopGridView.RefreshAllShownItem();
	}

	// IMPORTANT: This callback signature differs from LoopListView2
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
			cell.SetItemData(itemData);
			cell.OnCellClicked = OnCellClicked;
		}

		return itemObj;
	}

	private void OnCellClicked(BaseItem clickedItem)
	{
		Debug.Log($"[InventoryLeftPanelView] Clicked item: {clickedItem?.DisplayName}");

		// Publish an event or notify parent
		_eventBus.Publish(new ItemSelectedEvent { SelectedItem = clickedItem });
	}
}
