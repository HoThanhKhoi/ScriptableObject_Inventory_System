using System.Collections.Generic;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.DomainModels;
using InventorySystem.Data.Enums;
using System;
using System.Diagnostics;
using System.Linq;
using InventorySystem.Data;
using VContainer;

namespace InventorySystem.Core.Managers
{
	public class InventoryManager : IInventoryService
	{
		private readonly IEventBus _eventBus;
		//private readonly Dictionary<SlotIdEnum, SlotModel> _equippedSlots;
		//private readonly Dictionary<(SlotIdEnum, int), SubSlotModel> _subSlotModelDictionary = new Dictionary<(SlotIdEnum, int), SubSlotModel>();
		// <(slotId, subSlotIndex)>

		private readonly List<SlotModel> _equippedSlots;
		private readonly List<SubSlotModel> _equippedSubSlots;

		private readonly List<BaseItem> _allItems;
		private readonly IEquipmentSlotService _slotService; // We need this to get capacity, etc.

		private SlotIdEnum _currentSlotId;
		private int _currentSubSlotId;
		private bool _onSubSlotClickedStatus;

		
		public InventoryManager(IEventBus eventBus, IEquipmentSlotService slotService)
		{
			_eventBus = eventBus;
			_slotService = slotService;
			_equippedSlots = new List<SlotModel>();
			_equippedSubSlots = new List<SubSlotModel>();
			_allItems = new List<BaseItem>();
		}

		public void InitializeInventory()
		{
			foreach (SlotIdEnum slotId in Enum.GetValues(typeof(SlotIdEnum)))
			{
				GetOrCreateSlot(slotId);

				EquipmentSlotDefinition slotDefinition = _slotService.GetSlotDefinitionByIdFromList(slotId);

				for (int i = 0; i < slotDefinition.Capacity; i++)
				{
					GetOrCreateSubSlot(slotId, i);
				}
			}
		}

		public void AddItem(BaseItem item)
		{
			if (item == null) return;
			_allItems.Add(item);
			_eventBus.Publish(new ItemAddedEvent { Item = item });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public void RemoveItem(BaseItem item)
		{
			if (item == null) return;
			if (_allItems.Remove(item))
			{
				// If it was equipped, remove it
				foreach (SlotModel slot in _equippedSlots)
				{
					if (slot.EquippedItems.Contains(item))
					{
						slot.EquippedItems.Remove(item);
						_eventBus.Publish(new ItemUnequippedEvent { Item = item, SlotId = slot.SlotId });
					}
				}
				_eventBus.Publish(new ItemRemovedEvent { Item = item });
				_eventBus.Publish(new InventoryUpdatedEvent());
			}
		}

		// Overload for multi-sub-slot
		public void EquipItem(BaseItem item, SlotIdEnum slotId, int subSlotId)
		{
			//if (!CanEquipItem(item, slotId)) return;

			SlotModel slot = GetOrCreateSlot(slotId);

			// Ensure subSlotIndex is valid
			int capacity = _slotService.GetCapacity(slotId);
			// If subSlotIndex is out of range, we clamp it
			if (subSlotId < 0) subSlotId = 0;
			if (subSlotId >= capacity) subSlotId = capacity - 1;

			//// If the sub-slot doesn't exist yet, we expand the list up to subSlotIndex
			//while (slot.EquippedItems.Count <= subSlotId)
			//{
			//	slot.EquippedItems.Add(null);
			//}

			// Override the existing item at subSlotIndex
			BaseItem oldItem = slot.EquippedItems[subSlotId];
			slot.EquippedItems[subSlotId] = item;

			GetSubSlot(slotId, subSlotId).EquippedItem = item;

			// If oldItem != null, that item is replaced
			// Publish events
			if (oldItem != null)
			{
				_eventBus.Publish(new ItemUnequippedEvent { Item = oldItem, SlotId = slotId });
			}

			_eventBus.Publish(new ItemEquippedEvent { SubSlotModel = GetSubSlot(slotId, subSlotId) });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public void UnequipItem(SlotIdEnum slotId, int subSlotId)
		{
			var slot = GetSlot(slotId);
			if (slot == null) return;

			if (subSlotId < 0 || subSlotId >= slot.EquippedItems.Count) return;

			var oldItem = slot.EquippedItems[subSlotId];
			if (oldItem == null) return;

			slot.EquippedItems[subSlotId] = null;
			_eventBus.Publish(new ItemUnequippedEvent { Item = oldItem, SlotId = slotId });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public BaseItem[] GetAllItems()
		{
			return _allItems.ToArray();
		}

		public SlotModel GetSlot(SlotIdEnum slotId)
		{
			return _equippedSlots.FirstOrDefault(s => s.SlotId == slotId);
		}

		public SubSlotModel GetSubSlot(SlotIdEnum slotId, int subSlotId)
		{
			return _equippedSubSlots.FirstOrDefault(s => s.SlotId == slotId && s.SubSlotId == subSlotId);
		}

		
		// Helper
		private SubSlotModel GetOrCreateSubSlot(SlotIdEnum slotId, int subSlotId)
		{
			SubSlotModel subSlot = GetSubSlot(slotId, subSlotId);
			if (subSlot == null)
			{
				subSlot = new SubSlotModel(slotId, subSlotId, null);
				_equippedSubSlots.Add(subSlot);
			}
			return subSlot;
		}
		
		private SlotModel GetOrCreateSlot(SlotIdEnum slotId)
		{
			SlotModel slot = GetSlot(slotId);
			if (slot == null)
			{
				slot = new SlotModel(slotId);
				_equippedSlots.Add(slot);
			}
			return slot;
		}

		public bool SetSubSlotClickedStatus(bool status)
		{
			return _onSubSlotClickedStatus = status;
		}

		public bool GetSubSlotClickedStatus()
		{
			return _onSubSlotClickedStatus;
		}

		public int GetCurrentSubSlotId()
		{
			return _currentSubSlotId;
		}

		public void SetCurrentSubSlotId(int subSlotId)
		{
			_currentSubSlotId = subSlotId;
		}

		public SlotIdEnum GetCurrentSlotId()
		{
			return _currentSlotId;
		}

		public void SetCurrentSlotId(SlotIdEnum slotId)
		{
			_currentSlotId = slotId;
		}

		//public bool IsEquipped(string slotId)
		//{
		//	var slot = GetSlot(slotId);
		//	if (slot == null) return false;
		//	// If any sub-slot has an item, consider it "equipped"
		//	foreach (var it in slot.EquippedItems)
		//	{
		//		if (it != null) return true;
		//	}
		//	return false;
		//}

		//public BaseItem GetEquippedItem(string slotId)
		//{
		//	// This old method returns the first item if multi-sub-slot
		//	var slot = GetSlot(slotId);
		//	if (slot != null && slot.EquippedItems.Count > 0)
		//	{
		//		return slot.EquippedItems[0];
		//	}
		//	return null;
		//}

		//public bool CanEquipItem(BaseItem item, string slotId)
		//{
		//	// Basic check: if item category is allowed
		//	if (item == null) return false;
		//	return _slotService.IsItemAllowedInSlot(item.Category, slotId);
		//}

	}
}
