using UnityEngine;
using VContainer;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;
using InventorySystem.Data;
using System.Collections.Generic;
using InventorySystem.Data.Enums;
using InventorySystem.UI.Screens.InventoryScreen;

namespace InventorySystem.TestsAndSamples
{
	public class InventoryAutoPopulator : MonoBehaviour
	{
		[SerializeField] private BaseItem[] _itemsToAdd;
		private Dictionary<SlotIdEnum, EquipmentSlotDefinition> _slotDefinitions;

		private IInventoryService _inventoryService;
		private IEquipmentSlotService _slotService;

		[Inject]
		public void Construct(IInventoryService inventoryService, IEquipmentSlotService slotService)
		{
			if (inventoryService == null || slotService == null) return;
			_inventoryService = inventoryService;
			_slotService = slotService;

			_inventoryService.InitializeInventory();
			InitializeInventoryAutoPopulator();
		}

		private void OnEnable()
		{
		}

		private void Start()
		{
		}

		private void InitializeInventoryAutoPopulator()
		{
			Debug.Log($"InventoryAutoPopulator: InventoryService: {_inventoryService == null}, SlotService: {_slotService == null}");
			foreach (var item in _itemsToAdd)
			{
				if (item != null)
				{
					_inventoryService.AddItem(item);
				}
			}

			Debug.Log($"{_inventoryService.GetAllItems().Length} items in inventory");

			GetSlotDefinitionInHierarchy();

			Debug.Log($"[InventoryAutoPopulator] 1. slots in hierarchy {_slotDefinitions.Count}");
			foreach (var slot in _slotDefinitions)
			{
				Debug.Log($"[InventoryAutoPopulator] 2. Slot: {slot.Key}, Definition: {slot.Value}");
				if (slot.Value != null)
				{
					_slotService.AddSlotDefinition(slot.Key, slot.Value);
					Debug.Log($"[InventoryAutoPopulator] 3. Added slot: {slot.Key}");
				}
			}
		}

		private void GetSlotDefinitionInHierarchy()
		{
			_slotDefinitions = new Dictionary<SlotIdEnum, EquipmentSlotDefinition>();
			SlotView[] slotViews = FindObjectsByType<SlotView>(FindObjectsSortMode.None);

			foreach (SlotView slot in slotViews)
			{
				Debug.Log($"[InventoryAutoPopulator] Slot: {slot.EquipmentSlotDefinition.SlotId}, Definition: {slot.EquipmentSlotDefinition}");
				_slotDefinitions.Add(slot.EquipmentSlotDefinition.SlotId, slot.EquipmentSlotDefinition);
			}
		}
	}
}