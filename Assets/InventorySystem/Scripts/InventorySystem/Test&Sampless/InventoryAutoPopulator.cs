using UnityEngine;
using VContainer;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;
using InventorySystem.Data;
using System.Collections.Generic;
using InventorySystem.Data.Enums;

public class InventoryAutoPopulator : MonoBehaviour
{
	[SerializeField] private BaseItem[] _itemsToAdd;
	private Dictionary<SlotIdEnum, EquipmentSlotDefinition> _slotDefinitions;

	private IInventoryService _inventoryService;
	private IEquipmentSlotService _slotService;

	[Inject]
	public void Construct(IInventoryService inventoryService)
	{
		_inventoryService = inventoryService;
	}

	private void OnEnable()
	{
		_inventoryService.InitializeInventory();
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

		Debug.Log($"{_inventoryService.GetAllItems().Length} items in inventory");

		GetSlotDefinitionInHierarchy();

		foreach (var slot in _slotDefinitions)
		{
			if (slot.Value != null)
			{
				_slotService.AddSlotDefinition(slot.Key, slot.Value);
			}
		}
	}

	private void GetSlotDefinitionInHierarchy()
	{
		_slotDefinitions = new Dictionary<SlotIdEnum, EquipmentSlotDefinition>();
		EquipmentSlotDefinition[] slotDefinitions = FindObjectsByType<EquipmentSlotDefinition>(FindObjectsSortMode.None);
		foreach (var slot in slotDefinitions)
		{
			_slotDefinitions.Add(slot.SlotId, slot);
		}
	}
}