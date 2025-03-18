using System.Collections.Generic;
using InventorySystem.Data.Items;
using InventorySystem.Data.Enums;

namespace InventorySystem.Core.Interfaces
{
	public interface IFilterService
	{
		IEnumerable<BaseItem> FilterByCategory(IEnumerable<BaseItem> items, ItemCategoryEnum category);
		IEnumerable<BaseItem> FilterByMultipleCategories(IEnumerable<BaseItem> items, List<ItemCategoryEnum> categories);
		IEnumerable<BaseItem> SortAlphabetically(IEnumerable<BaseItem> items);
	}
}
