using UnityEngine;
using UnityEngine.UI;
using InventorySystem.Data.Items;
using InventorySystem.Data;
using InventorySystem.Infrastructure.Events;
using VContainer;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	// Represents one equip slot in the UI (weapon, consumable, zodiac, etc.).
	public class SlotView : MonoBehaviour
	{
		[SerializeField] private Image _iconImage;
		[SerializeField] private Button _slotButton;
		[SerializeField] private EquipmentSlotDefinition _equipmentSlotDefinition;
		private BaseItem _equippedItem;

		private IEventBus _eventBus;

		public EquipmentSlotDefinition EquipmentSlotDefinition => _equipmentSlotDefinition;

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

		public void HandleSlotHovered()
		{
			_eventBus.Publish(new OnSlotHoveredEvent { SelectedItem = _equippedItem });
		}

		public void HandleSlotClick()
		{
			_eventBus.Publish(new OnSlotClickedEvent { SelectedItem = _equippedItem, SlotDefinition = _equipmentSlotDefinition});
		}

	}
}
