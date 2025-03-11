using System.Collections.Generic;
using System.Linq;
using InventorySystem.Data.Items;
using InventorySystem.Data.Enums;
using InventorySystem.Core.Interfaces;


namespace InventorySystem.Core.Managers
{
	// Provides filtering and sorting utilities for items.
	public class FilterManager : IFilterService
	{
		public IEnumerable<BaseItem> FilterByCategory(IEnumerable<BaseItem> items, ItemCategory category)
		{
			return items.Where(i => i.Category == category);
		}

		public IEnumerable<BaseItem> FilterByMultipleCategories(IEnumerable<BaseItem> items, List<ItemCategory> categories)
		{
			return items.Where(i => categories.Contains(i.Category));
		}

		public IEnumerable<BaseItem> SortAlphabetically(IEnumerable<BaseItem> items)
		{
			return items.OrderBy(i => i.DisplayName);
		}

		public IEnumerable<BaseItem> FilterAndSort(IEnumerable<BaseItem> items, ItemCategory category)
		{
			var filtered = FilterByCategory(items, category);
			return SortAlphabetically(filtered);
		}

		// Overload for multiple categories
		public IEnumerable<BaseItem> FilterAndSort(IEnumerable<BaseItem> items, List<ItemCategory> categories)
		{
			var filtered = FilterByMultipleCategories(items, categories);
			return SortAlphabetically(filtered);
		}
	}
}
