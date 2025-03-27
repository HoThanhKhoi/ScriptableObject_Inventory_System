// InventoryManager.cs
using System.Collections.Generic;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.DomainModels;
using InventorySystem.Data.Enums;
using System;
using System.Diagnostics;

namespace InventorySystem.Core.Managers
{
	public class InventoryManager : IInventoryService
	{
		private readonly IEventBus _eventBus;
		private readonly Dictionary<SlotIdEnum, SlotModel> _equippedSlots;

		private readonly Dictionary<SubSlotModel, SubSlotModel> _subSlots;

		private readonly List<BaseItem> _allItems;
		private readonly IEquipmentSlotService _slotService; // We need this to get capacity, etc.
		
		private bool _onSubSlotClickedStatus;

		public InventoryManager(IEventBus eventBus, IEquipmentSlotService slotService)
		{
			_eventBus = eventBus;
			_slotService = slotService;
			_equippedSlots = new Dictionary<SlotIdEnum, SlotModel>();
			_allItems = new List<BaseItem>();
		}

		public void InitializeInventory() 
		{
			foreach(SlotIdEnum slotId in Enum.GetValues(typeof(SlotIdEnum)))
			{
				GetOrCreateSlot(slotId);
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
				foreach (var slot in _equippedSlots.Values)
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

			// If the sub-slot doesn't exist yet, we expand the list up to subSlotIndex
			while (slot.EquippedItems.Count <= subSlotId)
			{
				slot.EquippedItems.Add(null);
			}

			// Override the existing item at subSlotIndex
			BaseItem oldItem = slot.EquippedItems[subSlotId];
			slot.EquippedItems[subSlotId] = item;

			// If oldItem != null, that item is replaced
			// Publish events
			if (oldItem != null)
			{
				_eventBus.Publish(new ItemUnequippedEvent { Item = oldItem, SlotId = slotId });
			}

			_eventBus.Publish(new ItemEquippedEvent { Item = item, SlotId = slotId });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public void UnequipItem(SlotIdEnum slotId, int subSlotIndex)
		{
			var slot = GetSlot(slotId);
			if (slot == null) return;

			if (subSlotIndex < 0 || subSlotIndex >= slot.EquippedItems.Count) return;

			var oldItem = slot.EquippedItems[subSlotIndex];
			if (oldItem == null) return;

			slot.EquippedItems[subSlotIndex] = null;
			_eventBus.Publish(new ItemUnequippedEvent { Item = oldItem, SlotId = slotId });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public BaseItem[] GetAllItems()
		{
			return _allItems.ToArray();
		}



		public SlotModel GetSlot(SlotIdEnum slotId)
		{
			if (_equippedSlots == null) return null;
			if (!_equippedSlots.ContainsKey(slotId)) return null;

			_equippedSlots.TryGetValue(slotId, out SlotModel slot);
			return slot;
		}



		// Helper
		private SlotModel GetOrCreateSlot(SlotIdEnum slotId)
		{
			if (!_equippedSlots.TryGetValue(slotId, out SlotModel slot))
			{
				slot = new SlotModel(slotId);
				_equippedSlots[slotId] = slot;
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
