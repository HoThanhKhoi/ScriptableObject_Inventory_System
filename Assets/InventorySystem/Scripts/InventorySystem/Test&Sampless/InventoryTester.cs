// Suppose you have a test script
using UnityEngine;
using VContainer;
using VContainer.Unity;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;

public class InventoryTester : MonoBehaviour
{
	[Inject] private IInventoryService _inventoryService;

	[SerializeField] private BaseItem _testItem;

	private void Start()
	{
		if (_testItem != null)
		{
			_inventoryService.AddItem(_testItem);
		}
	}
}
