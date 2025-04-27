using System.Linq;
using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.UI {
	public class StealEnemyCells: MonoBehaviour {
		private LevelRoot _root;

		public void SetRoot(LevelRoot root) {
			_root = root;
		}
		private void OnCellCaptured(CellCapturedEvent gameEvent) {
			if (!gameEvent.Cell.Building.Value || gameEvent.PreviousOwner == null) {
				return;
			}
			if (gameEvent.Cell.Building.Value is not Castle castle) {
				return;
			}
			Destroy(castle.gameObject);
			gameEvent.Cell.SetBuilding(null);
			
			var cells = _root.Grid.Cells.Where(c => c.Value.Owner.Value == gameEvent.PreviousOwner);
			foreach ((Vector2Int position, Cell cell) in cells) {
				cell.Capture(gameEvent.By);
			}
		}
		private void OnEnable() {
			EventBus<CellCapturedEvent>.Event += OnCellCaptured;
		}
		private void OnDisable() {
			EventBus<CellCapturedEvent>.Event -= OnCellCaptured;
		}
	}
}