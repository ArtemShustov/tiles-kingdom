using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles {
	public class PlayGrid: MonoBehaviour {
		[SerializeField] private Grid _grid;
		
		private Dictionary<Vector2Int, Cell> _cells = new();

		public bool HasCell(Vector2Int position) {
			return _cells.ContainsKey(position);
		}
		public Cell GetCell(Vector2Int position) {
			_cells.TryGetValue(position, out Cell cell);
			return cell;
		}
		public bool TryGetCell(Vector2Int position, out Cell cell) {
			return _cells.TryGetValue(position, out cell);
		}
		public void SetCell(Vector2Int position, Cell cell) {
			_cells[position] = cell;
		}
		public void RemoveCell(Vector2Int position) {
			_cells.Remove(position);
		}

		public Vector3 GetCellCenterWorld(Vector2Int position) {
			return _grid.GetCellCenterWorld((Vector3Int)position);
		}
		public Vector2Int WorldToCell(Vector3 worldPosition) {
			return (Vector2Int)_grid.WorldToCell(worldPosition);
		}

		public IEnumerable<Cell> GetNeighbours(Vector2Int position) {
			var neighbours = new Vector2Int[] {
				new Vector2Int(0, 1),
				new Vector2Int(0, -1),
				new Vector2Int(1, 0),
				new Vector2Int(-1, 0),
			};
			for (int i = 0; i < neighbours.Length; i++) {
				var pos = position + neighbours[i];
				if (TryGetCell(pos, out var cell)) {
					yield return cell;
				}
			}
		}
		public IEnumerable<Cell> GetEightNeighbours(Vector2Int position) {
			var neighbours = new Vector2Int[] {
				new Vector2Int(0, 1),
				new Vector2Int(0, -1),
				new Vector2Int(1, 0),
				new Vector2Int(-1, 0),
				new Vector2Int(1, 1),
				new Vector2Int(1, -1),
				new Vector2Int(-1, 1),
				new Vector2Int(-1, -1),
			};
			for (int i = 0; i < neighbours.Length; i++) {
				var pos = position + neighbours[i];
				if (TryGetCell(pos, out var cell)) {
					yield return cell;
				}
			}
		}
	}
}