using Core.Events;
using UnityEngine;

namespace Game.Popups {
	public class ShowPopupEvent: IGameEvent {
		public Vector2 Position { get; private set; }
		public Color Color { get; private set; }
		public string Text { get; private set; }

		public ShowPopupEvent(Vector2 position, Color color, string text) {
			Position = position;
			Text = text;
			Color = color;
		}
	}
}