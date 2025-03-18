//using UnityEngine;
//using UnityEngine.UI;
//using VContainer;
//using InventorySystem.Infrastructure.Events;
//using InventorySystem.Data.Items;
//using TMPro;

//namespace InventorySystem.UI.Screens.InventoryScreen
//{
//	public class DescriptionView : MonoBehaviour
//	{
//		[SerializeField] private TextMeshProUGUI _itemNameText;
//		[SerializeField] private TextMeshProUGUI _itemDescriptionText;
//		[SerializeField] private Image _itemIconImage;

//		private IEventBus _eventBus;

//		[Inject]
//		public void Construct(IEventBus eventBus)
//		{
//			_eventBus = eventBus;
//			Debug.Log("ItemDescriptionPanelView: Injection succeeded!");
//		}

//		private void OnEnable()
//		{
//			_eventBus.Subscribe<ItemSelectedEvent>(OnItemSelected);
//			_eventBus.Subscribe<OnSlotHoveredEvent>(OnItemSelected);
//		}

//		private void OnDisable()
//		{
//			_eventBus.Unsubscribe<ItemSelectedEvent>(OnItemSelected);
//			_eventBus.Unsubscribe<OnSlotHoveredEvent>(OnItemSelected);

//		}

//		private void OnItemSelected(ItemSelectedEvent evt)
//		{
//			UpdateDescription(evt.SelectedItem);
//		}

//		private void UpdateDescription(BaseItem item)
//		{
//			if (item == null)
//			{
//				_itemNameText.text = "No Item Selected";
//				_itemDescriptionText.text = "";
//				_itemIconImage.enabled = false;
//				return;
//			}

//			_itemNameText.text = item.DisplayName;
//			_itemDescriptionText.text = item.Description;
//			if (item.Icon != null)
//			{
//				_itemIconImage.enabled = true;
//				_itemIconImage.sprite = item.Icon;
//			}
//			else
//			{
//				_itemIconImage.enabled = false;
//			}
//		}
//	}
//}
