namespace Game.Tiles.Buildings {
	public class Fence: Building {
		private Player _owner;
		
		private void OnCaptured() {
			var cell = Cell;
			Cell.SetBuilding(null);
			cell.Capture(_owner);
			Destroy(gameObject);
		}
		protected override void OnBind(Cell cell) {
			base.OnBind(cell);
			_owner = cell.Owner.Value;
			cell.Owner.ValueChanged += OnCaptured;
		}
		protected override void OnUnbind(Cell cell) {
			base.OnUnbind(cell);
			cell.Owner.ValueChanged -= OnCaptured;
		}
	}
}