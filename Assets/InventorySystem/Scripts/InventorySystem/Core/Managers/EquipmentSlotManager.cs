using System.Collections.Generic;
using InventorySystem.Core.DomainModels;
using InventorySystem.Core.Interfaces;
using InventorySystem.Data.Enums;

namespace InventorySystem.Core.Managers
{
	// Manages slot definitions & restrictions.
	public class EquipmentSlotManager : IEquipmentSlotService
	{
		// Maps slotId to SlotRestriction
		private Dictionary<string, SlotRestriction> _slotRestrictions;

		// Add capacity dictionary
		//private Dictionary<string, int> _slotCapacities;


		public EquipmentSlotManager()
		{
			_slotRestrictions = new Dictionary<string, SlotRestriction>();
			//_slotCapacities = new Dictionary<string, int>();

			RegisterSlot("weaponSlot", new List<ItemCategory> { ItemCategory.Weapon }, 1);
			RegisterSlot("skillGroup1Slot", new List<ItemCategory> { ItemCategory.SkillGroup1 }, 2);
			RegisterSlot("skillGroup2Slot", new List<ItemCategory> { ItemCategory.SkillGroup2 }, 2);
			RegisterSlot("consumableSlot", new List<ItemCategory> { ItemCategory.Consumable }, 10);

			RegisterSlot("zodiacRatSlot", new List<ItemCategory> { ItemCategory.ZodiacRat }, 1);
			RegisterSlot("zodiacOxSlot", new List<ItemCategory> { ItemCategory.ZodiacOx }, 1);
			RegisterSlot("zodiacTigerSlot", new List<ItemCategory> { ItemCategory.ZodiacTiger }, 1);
			RegisterSlot("zodiacRabbitSlot", new List<ItemCategory> { ItemCategory.ZodiacRabbit }, 1);
			RegisterSlot("zodiacDragonSlot", new List<ItemCategory> { ItemCategory.ZodiacDragon }, 1);
			RegisterSlot("zodiacSnakeSlot", new List<ItemCategory> { ItemCategory.ZodiacSnake }, 1);
			RegisterSlot("zodiacHorseSlot", new List<ItemCategory> { ItemCategory.ZodiacHorse }, 1);
			RegisterSlot("zodiacGoatSlot", new List<ItemCategory> { ItemCategory.ZodiacGoat }, 1);
			RegisterSlot("zodiacMonkeySlot", new List<ItemCategory> { ItemCategory.ZodiacMonkey }, 1);
			RegisterSlot("zodiacRoosterSlot", new List<ItemCategory> { ItemCategory.ZodiacRooster }, 1);
			RegisterSlot("zodiacDogSlot", new List<ItemCategory> { ItemCategory.ZodiacDog }, 1);
			RegisterSlot("zodiacPigSlot", new List<ItemCategory> { ItemCategory.ZodiacPig }, 1);


			// Example: You can programmatically register slot restrictions here
			// or load them from ScriptableObjects.
			// e.g. RegisterSlot("weaponSlot", new List<ItemCategory> { ItemCategory.Weapon });
		}

		public void RegisterSlot(string slotId, List<ItemCategory> allowedCategories, int capacity)
		{
			//_slotCapacities[slotId] = capacity;
			_slotRestrictions[slotId] = new SlotRestriction(slotId, allowedCategories, capacity);
		}


		public SlotRestriction GetRestriction(string slotId)
		{
			_slotRestrictions.TryGetValue(slotId, out var restriction);
			return restriction;
		}

		public int GetCapacity(string slotId)
		{
			if (_slotRestrictions.TryGetValue(slotId, out var restriction)) 
				return restriction.Capacity;
			return 1; // default
		}

		public bool IsItemAllowedInSlot(ItemCategory category, string slotId)
		{
			if (_slotRestrictions.TryGetValue(slotId, out var restriction))
			{
				return restriction.AllowedCategories.Contains(category);
			}
			return false;
		}

		public List<ItemCategory> GetAllowedCategories(string slotId)
		{
			if (_slotRestrictions.TryGetValue(slotId, out var restriction))
			{
				return restriction.AllowedCategories;
			}
			return new List<ItemCategory>();
		}
	}
}
