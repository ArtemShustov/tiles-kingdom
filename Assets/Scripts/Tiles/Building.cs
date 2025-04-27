using UnityEngine;

namespace Game.Tiles {
	public class Building: MonoBehaviour {
		[SerializeField] private int _baseCaptureCost = 0;
		
		public Cell Cell { get; private set; }
		public virtual int CaptureCost => _baseCaptureCost;

		public virtual bool CanBuildAt(PlayGrid grid, Vector2Int position) {
			return true;
		}
		
		public void Bind(Cell cell) {
			if (cell != null) {
				OnUnbind(cell);
			}
			Cell = cell;
			if (cell != null) {
				OnBind(cell);
			}
		}
		public void UnBind(Cell cell) {
			OnUnbind(cell);
			Cell = null;
		}

		protected virtual void OnBind(Cell cell) { }
		protected virtual void OnUnbind(Cell cell) { }
		
	}
}