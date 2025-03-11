// InventoryManager.cs
using System.Collections.Generic;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Items;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.DomainModels;
using InventorySystem.Data.Enums;

namespace InventorySystem.Core.Managers
{
	public class InventoryManager : IInventoryService
	{
		private readonly IEventBus _eventBus;
		private readonly Dictionary<string, InventorySlot> _equippedSlots;
		private readonly List<BaseItem> _inventoryItems;
		private readonly IEquipmentSlotService _slotService; // We need this to get capacity, etc.

		public InventoryManager(IEventBus eventBus, IEquipmentSlotService slotService)
		{
			_eventBus = eventBus;
			_slotService = slotService;
			_equippedSlots = new Dictionary<string, InventorySlot>();
			_inventoryItems = new List<BaseItem>();
		}

		public void AddItem(BaseItem item)
		{
			if (item == null) return;
			_inventoryItems.Add(item);
			_eventBus.Publish(new ItemAddedEvent { Item = item });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public void RemoveItem(BaseItem item)
		{
			if (item == null) return;
			if (_inventoryItems.Remove(item))
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

		public bool CanEquipItem(BaseItem item, string slotId)
		{
			// Basic check: if item category is allowed
			if (item == null) return false;
			return _slotService.IsItemAllowedInSlot(item.Category, slotId);
		}

		// Overload for multi-sub-slot
		public void EquipItem(BaseItem item, string slotId, int subSlotIndex)
		{
			if (!CanEquipItem(item, slotId)) return;

			var slot = GetOrCreateSlot(slotId);
			int capacity = _slotService.GetCapacity(slotId);

			// If subSlotIndex is out of range, we clamp it
			if (subSlotIndex < 0) subSlotIndex = 0;
			if (subSlotIndex >= capacity) subSlotIndex = capacity - 1;

			// If the sub-slot doesn't exist yet, we expand the list up to subSlotIndex
			while (slot.EquippedItems.Count <= subSlotIndex)
			{
				slot.EquippedItems.Add(null);
			}

			// Override the existing item at subSlotIndex
			var oldItem = slot.EquippedItems[subSlotIndex];
			slot.EquippedItems[subSlotIndex] = item;

			// If oldItem != null, that item is replaced
			// Publish events
			if (oldItem != null)
			{
				_eventBus.Publish(new ItemUnequippedEvent { Item = oldItem, SlotId = slotId });
			}

			_eventBus.Publish(new ItemEquippedEvent { Item = item, SlotId = slotId });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		// 2-parameter version for old code
		public void EquipItem(BaseItem item, string slotId)
		{
			EquipItem(item, slotId, 0);
		}

		public void UnequipItem(string slotId, int subSlotIndex)
		{
			var slot = GetSlot(slotId);
			if (slot == null) return;

			if (subSlotIndex < 0 || subSlotIndex >= slot.EquippedItems.Count) return;

			var removedItem = slot.EquippedItems[subSlotIndex];
			if (removedItem == null) return;

			slot.EquippedItems[subSlotIndex] = null;
			_eventBus.Publish(new ItemUnequippedEvent { Item = removedItem, SlotId = slotId });
			_eventBus.Publish(new InventoryUpdatedEvent());
		}

		public void UnequipItem(string slotId)
		{
			// If you call the old version, we remove sub-slot 0
			UnequipItem(slotId, 0);
		}

		public bool IsEquipped(string slotId)
		{
			var slot = GetSlot(slotId);
			if (slot == null) return false;
			// If any sub-slot has an item, consider it "equipped"
			foreach (var it in slot.EquippedItems)
			{
				if (it != null) return true;
			}
			return false;
		}

		public BaseItem GetEquippedItem(string slotId)
		{
			// This old method returns the first item if multi-sub-slot
			var slot = GetSlot(slotId);
			if (slot != null && slot.EquippedItems.Count > 0)
			{
				return slot.EquippedItems[0];
			}
			return null;
		}

		public BaseItem[] GetAllItems()
		{
			return _inventoryItems.ToArray();
		}

		// Helper
		private InventorySlot GetOrCreateSlot(string slotId)
		{
			if (!_equippedSlots.TryGetValue(slotId, out var slot))
			{
				slot = new InventorySlot(slotId, true);
				_equippedSlots[slotId] = slot;
			}
			return slot;
		}

		private InventorySlot GetSlot(string slotId)
		{
			_equippedSlots.TryGetValue(slotId, out var slot);
			return slot;
		}
	}
}
