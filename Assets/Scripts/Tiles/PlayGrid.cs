using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles {
	public class PlayGrid: MonoBehaviour {
		[SerializeField] private Grid _grid;
		
		private Dictionary<Vector2Int, Cell> _cells = new();
		
		public IReadOnlyDictionary<Vector2Int, Cell> Cells => _cells;

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
		public Cell[] GetConnected(Vector2Int start, Func<Cell, bool> filter) {
			if (!TryGetCell(start, out var startCell)) {
				return Array.Empty<Cell>();
			}

			var visited = new HashSet<Vector2Int>();
			var queue = new Queue<Vector2Int>();
			var result = new List<Cell>();
			
			queue.Enqueue(start);

			while (queue.Count > 0) {
				var current = queue.Dequeue();

				if (visited.Contains(current)) {
					continue;
				}
				visited.Add(current);
				if (TryGetCell(current, out var cell) && filter(cell)) {
					result.Add(cell);
				}

				foreach (var neighbour in GetNeighbours(current)) {
					queue.Enqueue(neighbour.Position);
				}
			}

			return result.ToArray();
		}
	}
}