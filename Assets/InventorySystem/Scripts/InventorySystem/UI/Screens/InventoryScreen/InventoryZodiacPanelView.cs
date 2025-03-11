using UnityEngine;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Optional script if you want a separate panel to hold 12 zodiac slots.
	// Otherwise, you can just place 12 InventorySlotView objects in the scene.
	public class InventoryZodiacPanelView : MonoBehaviour
	{
		[SerializeField] private InventorySlotView[] _zodiacSlots;

		public void Init(System.Action<string> onSlotClicked)
		{
			foreach (var slotView in _zodiacSlots)
			{
				// For example, each slotView might have a name "zodiacRatSlot", etc.
				slotView.Init(slotView.name, onSlotClicked);
			}
		}
	}
}
