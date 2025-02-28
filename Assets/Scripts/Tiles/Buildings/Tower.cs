using Game.Tiles.Effects;
using UnityEngine;

namespace Game.Tiles.Buildings {
	public class Tower: Building {
		[SerializeField] private int _defence = 1;
		private TowerEffect _effect;

		private void Awake() {
			_effect = new TowerEffect(_defence, null);
		}
		
		private void OnOwnerChanged() {
			_effect.Owner = Cell.Owner.Value;
		}
		protected override void OnBind(Cell cell) {
			base.OnBind(cell);
			cell.Owner.ValueChanged += OnOwnerChanged;
			foreach (var neighbour in cell.Grid.GetEightNeighbours(cell.Position)) {
				neighbour.AddEffect(_effect);
			}
		}
		protected override void OnUnbind(Cell cell) {
			base.OnUnbind(cell);
			cell.Owner.ValueChanged -= OnOwnerChanged;
			foreach (var neighbour in cell.Grid.GetEightNeighbours(cell.Position)) {
				neighbour.RemoveEffect(_effect);
			}
		}
	}
}