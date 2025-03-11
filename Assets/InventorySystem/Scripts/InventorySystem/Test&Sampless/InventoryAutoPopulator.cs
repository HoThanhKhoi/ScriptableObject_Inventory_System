using UnityEngine;
using VContainer;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;

public class InventoryAutoPopulator : MonoBehaviour
{
	[SerializeField] private BaseItem[] _itemsToAdd;

	private IInventoryService _inventoryService;

	[Inject]
	public void Construct(IInventoryService inventoryService)
	{
		_inventoryService = inventoryService;
	}

	private void Start()
	{
		foreach (var item in _itemsToAdd)
		{
			if (item != null)
			{
				_inventoryService.AddItem(item);
			}
		}
	}
}
