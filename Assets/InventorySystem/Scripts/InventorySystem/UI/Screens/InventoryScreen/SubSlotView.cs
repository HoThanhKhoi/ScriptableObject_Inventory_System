using UnityEngine;
using UnityEngine.UI;
using InventorySystem.Data.Items;
using InventorySystem.Data;
using InventorySystem.Infrastructure.Events;
using VContainer;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Represents one equip slot in the UI (weapon, consumable, zodiac, etc.).
	public class SubSlotView : MonoBehaviour
	{
		[SerializeField] private Image _iconImage;
		[SerializeField] private Button _slotButton;
		private BaseItem _equippedItem;

		private IEventBus _eventBus;

		[Inject]
		public void Construct(IEventBus eventBus)
		{
			_eventBus = eventBus;
		}

		private void Awake()
		{

		}

		private void Start()
		{
			//_slotButton.onClick.AddListener(HandleSlotClick);
			//_slotButton.OnPointerUp.AddListener(HandleSlotHovered);
		}

		public void HandleSubSlotHovered()
		{
			//_eventBus.Publish(new OnSubSlotHoveredEvent { SelectedItem = _equippedItem });
		}

		public void HandleSubSlotRightClick()
		{
			//_eventBus.Publish(new OnSubSlotRightClickedEvent { SelectedItem = _equippedItem, SlotDefinition = _equipmentSlotDefinition });
		}

		public void HandleSubSlotLeftClick()
		{
			//_eventBus.Publish(new OnSubSlotLeftClickedEvent { SelectedItem = _equippedItem, SlotDefinition = _equipmentSlotDefinition });
		}

		////private void HandleSlotHovered()
		//{
		//	//_eventBus.Publish(new OnSlotHoveredEvent { SelectedItem = clickedItem });
		//}

		//public void Init(string slotId, System.Action<string> onSlotClicked)
		//{
		//	_slotId = slotId;
		//	_onSlotClicked = onSlotClicked;
		//	if (_slotButton != null)
		//	{
		//		_slotButton.OnPointerEnter.AddListener(HandleSlotClick);
		//	}
		//}

		//public void SetEquippedItem(BaseItem item)
		//{
		//	_equippedItem = item;
		//	if (_iconImage == null) return;

		//	if (item == null)
		//	{
		//		_iconImage.enabled = false;
		//	}
		//	else
		//	{
		//		_iconImage.enabled = true;
		//		_iconImage.sprite = item.Icon;
		//	}
		//}

	}
}
