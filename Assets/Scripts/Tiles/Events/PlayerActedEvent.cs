using Core.Events;

namespace Game.Tiles.Events {
	public class PlayerActedEvent: IGameEvent {
		public ActionType Type { get; private set; }

		public PlayerActedEvent(ActionType type) {
			Type = type;
		}

		public enum ActionType {
			None, Capture, Build
		}
	}
}