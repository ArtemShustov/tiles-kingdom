using System;

namespace Game.Tiles.Buildings {
	public class Castle: Building {
		public event Action<Player> Captured;

		private void OnCaptured() {
			Captured?.Invoke(Cell.Owner.Value);
		}
		protected override void OnBind(Cell cell) {
			cell.Owner.ValueChanged += OnCaptured;
		}
		protected override void OnUnbind(Cell cell) {
			cell.Owner.ValueChanged -= OnCaptured;
		}
	}
}