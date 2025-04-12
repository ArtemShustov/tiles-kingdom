using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Game.Tiles {
	public class GridPathFinder {
		private PlayGrid _grid;

		public GridPathFinder(PlayGrid grid) {
			_grid = grid;
		}

		public Cell GetCellNearOwned(Cell start, Player owner) => GetCellsNearOwned(start, owner).GetRandom();
		public Cell[] GetCellsNearOwned(Cell start, Player owner) {
			var directions = new Vector2Int[] {
				new Vector2Int(1, 0),
				new Vector2Int(-1, 0),
				new Vector2Int(0, 1),
				new Vector2Int(0, -1)
			};

			var visited = new HashSet<Vector2Int>();
			var queue = new Queue<Cell>();
			var candidates = new List<Cell>();
			queue.Enqueue(start);
			visited.Add(start.Position);

			while (queue.Count > 0) {
				var current = queue.Dequeue();

				foreach (var dir in directions) {
					var neighborPos = current.Position + dir;

					if (!_grid.HasCell(neighborPos) || visited.Contains(neighborPos)) {
						continue;
					}

					var neighborCell = _grid.GetCell(neighborPos);

					visited.Add(neighborPos);
					if (neighborCell.Owner.Value == owner) {
						queue.Enqueue(neighborCell);
					} else {
						candidates.Add(neighborCell);
					}
				}
			}

			if (candidates.Count == 0) {
				return null;
			}
			return candidates.ToArray();
		}
		public bool HasPath(Cell start, Cell end, Player owner) {
			if (start == null || end == null || owner == null) {
				return false;
			}

			var directions = new Vector2Int[] {
				new Vector2Int(1, 0),
				new Vector2Int(-1, 0),
				new Vector2Int(0, 1),
				new Vector2Int(0, -1)
			};

			var visited = new HashSet<Vector2Int>();
			var queue = new Queue<Cell>();

			queue.Enqueue(start);
			visited.Add(start.Position);

			while (queue.Count > 0) {
				var current = queue.Dequeue();
				
				if (current.Position == end.Position) {
					return true;
				}

				foreach (var dir in directions) {
					var neighborPos = current.Position + dir;

					if (!_grid.HasCell(neighborPos) || visited.Contains(neighborPos)) {
						continue;
					}

					var neighborCell = _grid.GetCell(neighborPos);
					if (neighborCell.Position == end.Position) {
						return true;
					}
					
					if (neighborCell.Owner.Value == owner) {
						visited.Add(neighborPos);
						queue.Enqueue(neighborCell);
					}
				}
			}
			
			return false;
		}
	}
}