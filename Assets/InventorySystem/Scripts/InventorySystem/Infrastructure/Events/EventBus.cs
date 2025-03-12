using System;
using System.Collections.Generic;
using VContainer;


namespace InventorySystem.Infrastructure.Events
{
	// Concrete event bus that stores event subscriptions by type.
	public class EventBus : IEventBus
	{
		private Dictionary<Type, object> _subscribers = new Dictionary<Type, object>();

		[Inject]
		public EventBus()
		{
			_subscribers = new Dictionary<Type, object>();
		}

		public void Subscribe<T>(Action<T> callback)
		{
			var eventType = typeof(T);
			if (!_subscribers.TryGetValue(eventType, out var existing))
			{
				_subscribers[eventType] = callback;
			}
			else
			{
				_subscribers[eventType] = (Action<T>)existing + callback;
			}
		}

		public void Unsubscribe<T>(Action<T> callback)
		{
			var eventType = typeof(T);
			if (_subscribers.TryGetValue(eventType, out var existing))
			{
				_subscribers[eventType] = (Action<T>)existing - callback;
			}
		}

		public void Publish<T>(T eventData)
		{
			var eventType = typeof(T);
			if (_subscribers.TryGetValue(eventType, out var existing))
			{
				var action = (Action<T>)existing;
				action?.Invoke(eventData);
			}
		}
	}
}
