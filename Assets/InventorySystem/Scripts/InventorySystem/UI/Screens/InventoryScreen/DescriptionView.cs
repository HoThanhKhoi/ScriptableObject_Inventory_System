using UnityEngine;
using UnityEngine.UI;
using VContainer;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Data.Items;
using TMPro;
using System;

namespace InventorySystem.UI.Screens.InventoryScreen
{
	public class DescriptionView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _itemNameText;
		[SerializeField] private TextMeshProUGUI _itemDescriptionText;
		[SerializeField] private Image _itemIconImage;

		private IEventBus _eventBus;

		[Inject]
		public void Construct(IEventBus eventBus)
		{
			_eventBus = eventBus;
			Debug.Log("ItemDescriptionPanelView: Injection succeeded!");
		}

		private void OnEnable()
		{
			_eventBus.Subscribe<ItemHoveredEvent>(OnItemHovered);
			_eventBus.Subscribe<ItemSelectedEvent>(OnItemSelected);
			_eventBus.Subscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);
			_eventBus.Subscribe<OnSubSlotRightClickedEvent>(OnSubSlotRightClicked);
		}

		private void OnDisable()
		{
			_eventBus.Subscribe<ItemHoveredEvent>(OnItemHovered);
			_eventBus.Unsubscribe<ItemSelectedEvent>(OnItemSelected);
			_eventBus.Unsubscribe<OnSubSlotHoveredEvent>(OnSubSlotHovered);
			_eventBus.Subscribe<OnSubSlotLeftClickedEvent>(OnSubSlotLeftClicked);
			_eventBus.Subscribe<OnSubSlotRightClickedEvent>(OnSubSlotRightClicked);
		}

		private void OnItemHovered(ItemHoveredEvent e)
		{
			UpdateDescription(e.SelectedItem);
		}

		private void OnItemSelected(ItemSelectedEvent e)
		{
			UpdateDescription(e.SelectedItem);
		}

		private void OnSubSlotHovered(OnSubSlotHoveredEvent e)
		{
			UpdateDescription(e.SelectedItem);
		}

		private void OnSubSlotLeftClicked(OnSubSlotLeftClickedEvent e)
		{
			UpdateDescription(e.SelectedItem);
		}

		private void OnSubSlotRightClicked(OnSubSlotRightClickedEvent e)
		{
			UpdateDescription(e.SelectedItem);
		}

		private void UpdateDescription(BaseItem item)
		{
			if (item == null)
			{
				_itemNameText.text = "No Item Selected";
				_itemDescriptionText.text = "";
				_itemIconImage.enabled = false;
				return;
			}

			_itemNameText.text = item.DisplayName;
			_itemDescriptionText.text = item.Description;
			if (item.Icon != null)
			{
				_itemIconImage.enabled = true;
				_itemIconImage.sprite = item.Icon;
			}
			else
			{
				_itemIconImage.enabled = false;
			}
		}
	}
}
