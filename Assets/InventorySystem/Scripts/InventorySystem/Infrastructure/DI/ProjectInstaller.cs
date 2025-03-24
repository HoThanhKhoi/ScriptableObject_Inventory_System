using UnityEngine;
using VContainer;
using VContainer.Unity;
using InventorySystem.Infrastructure.Events;
using InventorySystem.Core.Interfaces;
using InventorySystem.Core.Managers;
using InventorySystem.UI.Screens.InventoryScreen;

namespace InventorySystem.Infrastructure.DI
{
	// 1. Inherit from LifetimeScope

	public class ProjectInstaller : LifetimeScope
	{
		// 2. Override the Configure method
		protected override void Configure(IContainerBuilder builder)
		{
			

			// Register EventBus as a singleton
			builder.Register<EventBus>(Lifetime.Singleton)
				   .As<IEventBus>();

			// Register InventoryManager as IInventoryService
			builder.Register<InventoryManager>(Lifetime.Singleton)
				   .As<IInventoryService>();

			// Register EquipmentSlotManager as IEquipmentSlotService
			builder.Register<EquipmentSlotManager>(Lifetime.Singleton)
				   .As<IEquipmentSlotService>();

			// Register FilterManager as IFilterService
			builder.Register<FilterManager>(Lifetime.Singleton)
				   .As<IFilterService>();

			builder.RegisterComponentInHierarchy<InventoryView>();
			builder.RegisterComponentInHierarchy<SlotView>();
			builder.RegisterComponentInHierarchy<EquipmentSubSlotView>();
			builder.RegisterComponentInHierarchy<InventoryAutoPopulator>();
			

			//builder.RegisterComponentInHierarchy<InventoryMainView>();
			//builder.RegisterComponentInHierarchy<InventoryLeftPanelView>();
			//builder.RegisterComponentInHierarchy<ItemDescriptionPanelView>();
			//builder.RegisterComponentInHierarchy<InventoryAutoPopulator>();

			//builder.RegisterComponentInHierarchy<InventoryTester>();
			//builder.RegisterComponentInHierarchy<EquippableInventoryScrollView>();

			//builder.RegisterComponentInHierarchy<EventBus>();
			//builder.RegisterComponentInHierarchy<InventoryManager>();
			//builder.RegisterComponentInHierarchy<EquipmentSlotManager>();
			//builder.RegisterComponentInHierarchy<FilterManager>();

			// If you have more classes to register, do it here.
		}
	}
}
