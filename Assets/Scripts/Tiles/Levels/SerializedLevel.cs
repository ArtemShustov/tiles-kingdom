using System;
using System.Collections.Generic;
using System.Linq;
using Game.Tiles.Buildings;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Serialized Level")]
	public class SerializedLevel: Level {
		[SerializeField] private Color[] _players;
		[SerializeField] private CellData[] _cells;
		
		public IReadOnlyList<Color> Players => _players;
		public IReadOnlyList<CellData> Cells => _cells;
		
		public override void Build(LevelRoot root) {
			ConfigureLevel(root);
			PlaceCells(root);
			PlaceBuildings(root);
			PlacePlayers(root);
		}

		protected virtual void ConfigureLevel(LevelRoot root) {
			// 
		}
		protected virtual void PlaceCells(LevelRoot root) {
			foreach (var cellData in _cells) {
				if (!root.Grid.HasCell(cellData.Position)) {
					root.PlaceEmptyCell(cellData.Position);
				}
			}
		}
		protected virtual void PlaceBuildings(LevelRoot root) {
			foreach (var cellData in _cells) {
				if (root.TryGetCell(cellData.Position, out var cell) && !cell.Building.Value) {
					switch (cellData.Building) { // FIXME: Bad impl
						case BuildingType.Castle:
							root.AttachCastle(cellData.Position);
							break;
						case BuildingType.Mine:
							root.AttachMine(cellData.Position);
							break;
						case BuildingType.Tower:
							root.AttachTower(cellData.Position);
							break;
						case BuildingType.Fence:
							root.AttachFence(cellData.Position);
							break;
						case BuildingType.None:
							break;
						default:
							Debug.LogWarning($"Unknown building type: {cellData.Building}");
							break;
					}
				}
			}
		}
		protected virtual void PlacePlayers(LevelRoot root) {
			var players = _players.Select(c => new Player(c)).ToArray();
			foreach (var cellData in _cells) {
				if (!cellData.Owned) {
					continue;
				}
				if (root.TryGetCell(cellData.Position, out var cell)) {
					var player = players[cellData.Owner];
					cell.Capture(player);
					if (cell.Building.Value is Castle castle) {
						// Player player, Castle castle
					}
				}
			}
		}
		
		#region Internal
		[Serializable]
		public class CellData {
			[field: SerializeField] public Vector2Int Position { get; private set; }
			[field: SerializeField] public BuildingType Building { get; private set; }
			[field: SerializeField] public bool Owned { get; private set; }
			[field: SerializeField] public int Owner { get; private set; }

			public CellData(Vector2Int position, BuildingType building, int owner = -1) {
				Position = position;
				Building = building;
				Owner = owner;
				Owned = owner != -1;
			}
		} 
		public enum BuildingType {
			None, Castle, Mine, Tower, Fence
		}
		#endregion
		#region Editor

		public static SerializedLevel Create(Cell[] cells) {
			var level = CreateInstance<SerializedLevel>();
			level._players = cells
				.Select(c => c.Owner.Value)
				.Distinct()
				.Where(p => p != null)
				.Select(p => p.Color)
				.ToArray();
			level._cells = cells.Select(ToData).ToArray();
			return level;

			CellData ToData(Cell cell) {
				var building = cell.Building.Value switch {
					Castle => BuildingType.Castle,
					Mine => BuildingType.Mine,
					Tower => BuildingType.Tower,
					Fence => BuildingType.Fence,
					_ => BuildingType.None
				};
				var owner = cell.Owner.Value == null ? -1 : IndexOf(cell.Owner.Value.Color);
				return new CellData(cell.Position, building, owner);
			}

			int IndexOf(Color player) {
				return Array.IndexOf(level._players, player);
			} 
		}
		#endregion
	}
}