using System;

namespace InventorySystem.Infrastructure.Events
{
	// A simple pub-sub event bus interface.
	public interface IEventBus
	{
		void Subscribe<T>(Action<T> callback);
		void Unsubscribe<T>(Action<T> callback);
		void Publish<T>(T eventData);
	}
}
