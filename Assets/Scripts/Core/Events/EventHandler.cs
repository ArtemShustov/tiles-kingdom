namespace Core.Events {
	public delegate void EventHandler<T>(T gameEvent) where T: IGameEvent; 
}