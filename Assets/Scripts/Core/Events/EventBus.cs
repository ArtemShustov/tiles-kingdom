namespace Core.Events {
	public static class EventBus<T> where T: IGameEvent {
		private static event EventHandler<T> _event;
		
		public static event EventHandler<T> Event {
			add { _event += value; }
			remove { _event -= value; }
		}

		public static void Raise(T gameEvent) {
			_event?.Invoke(gameEvent);
		}
		
		public static void AddHandler(EventHandler<T> handler) {
			_event += handler;
		}
		public static void RemoveHandler(EventHandler<T> handler) {
			_event -= handler;
		}
	}
}