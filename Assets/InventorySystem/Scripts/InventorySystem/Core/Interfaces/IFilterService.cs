using System.Collections.Generic;
using InventorySystem.Data.Items;
using InventorySystem.Data.Enums;

namespace InventorySystem.Core.Interfaces
{
	public interface IFilterService
	{
		IEnumerable<BaseItem> FilterByCategory(IEnumerable<BaseItem> items, ItemCategory category);
		IEnumerable<BaseItem> FilterByMultipleCategories(IEnumerable<BaseItem> items, List<ItemCategory> categories);
		IEnumerable<BaseItem> SortAlphabetically(IEnumerable<BaseItem> items);
	}
}
