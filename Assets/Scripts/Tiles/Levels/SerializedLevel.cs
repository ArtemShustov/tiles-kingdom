using System;
using System.Collections.Generic;
using Game.Tiles.Buildings;
using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels {
	public class SerializedLevel: Level {
		[SerializeField] private BonusTable _playerLogisticTable = new BonusTable(12, 6, 3);
		[SerializeField] private BonusTable _playerBonusTable = new BonusTable(20, 10, 5);
		[SerializeField] private BonusTable _enemyBonusTable = new BonusTable(0, 5, 10);
		[SerializeField] private CellData[] _cells;
		
		public override void Build(LevelRoot root) {
			foreach (var cellData in _cells) {
				root.PlaceEmptyCell(cellData.Position);
			}
			
			PlaceBuildings(root);
			PlaceEnemies(root);
			PlacePlayer(root);
			
			root.gameObject.AddComponent<ReloadSceneOnEnd>();
		}
		private void PlacePlayer(LevelRoot root) {
			foreach (var cellData in _cells) {
				if (cellData.OwnerType == OwnerType.Player) {
					var cell = root.GetCell(cellData.Position);
					cell.Capture(root.Player);
					if (cell.Building.Value is Castle castle) {
						root.SetPlayerCastle(castle);
					}
				}
			}
			
			root.Player.StrategyPoints.Add(_playerBonusTable.GetFor(PlayerProfile.Current.Difficulty));
			root.Player.LogisticsPoints.Add(_playerLogisticTable.GetFor(PlayerProfile.Current.Difficulty));
		}
		private void PlaceEnemies(LevelRoot root) {
			var enemies = new Dictionary<Color, Player>();
			foreach (var cellData in _cells) {
				if (cellData.OwnerType != OwnerType.Enemy) {
					continue;
				}
				if (!enemies.ContainsKey(cellData.Owner)) {
					enemies.Add(cellData.Owner, new Player(cellData.Owner, PlayerFlags.AI));
				}
				var player = enemies[cellData.Owner];
				var cell = root.GetCell(cellData.Position);
				cell.Capture(player);
				if (cell.Building.Value is Castle castle) {
					root.AddEnemy(player, castle);
					root.AddAI(player, castle);
					AddEnemyBonus(player);
				}
			}

			void AddEnemyBonus(Player enemy) {
				var bonusPoints = _enemyBonusTable.GetFor(PlayerProfile.Current.Difficulty);
				enemy.StrategyPoints.Add(bonusPoints);
			}
		}
		private void PlaceBuildings(LevelRoot root) {
			foreach (var cellData in _cells) { // TODO: Bad impl
				switch (cellData.Building) {
					case BuildingType.Castle:
						root.AttachCastle(cellData.Position);
						break;
					case BuildingType.Mine:
						root.AttachMine(cellData.Position);
						break;
					case BuildingType.Tower:
						root.AttachTower(cellData.Position);
						break;
					case BuildingType.TrojanHorse:
						root.AttackHorse(cellData.Position);
						break;
					case BuildingType.None:
						break;
					default:
						Debug.LogWarning($"Unknown building type: {cellData.Building}");
						break;
				}
			}
		}

		#region Editor
		public static SerializedLevel Create(Cell[] cells, Color player) {
			var datas = new List<CellData>();
			foreach (var cell in cells) {
				var building = cell.Building.Value switch {
					Castle => BuildingType.Castle,
					Mine => BuildingType.Mine,
					Tower => BuildingType.Tower,
					TrojanHorse => BuildingType.TrojanHorse,
					_ => BuildingType.None
				};
				if (cell.Owner.Value != null) {
					datas.Add(new CellData(cell.Owner.Value.Color, GetOwnerType(cell, player), cell.Position, building));
				} else {
					datas.Add(new CellData(cell.Position, building));
				}
			}
			var level = CreateInstance<SerializedLevel>();
			level._cells = datas.ToArray();
			return level;

			OwnerType GetOwnerType(Cell cell, Color playerColor) {
				if (cell.Owner.Value == null) {
					return OwnerType.None;
				} 
				return cell.Owner.Value.Color == playerColor ? OwnerType.Player : OwnerType.Enemy;
			}
		}
		#endregion

		#region Internal types
		[Serializable]
		public class CellData {
			[field: SerializeField] public Color Owner { get; private set; }
			[field: SerializeField] public Vector2Int Position { get; private set; }
			[field: SerializeField] public BuildingType Building { get; private set; }
			[field: SerializeField] public OwnerType OwnerType { get; private set; }

			public CellData(Color owner, OwnerType ownerType, Vector2Int position, BuildingType building) {
				Owner = owner;
				Building = building;
				Position = position;
				OwnerType = ownerType;
			}
			public CellData(Vector2Int position, BuildingType building) {
				OwnerType = OwnerType.None;
				Building = building;
				Position = position;
			}
		}
		public enum BuildingType {
			None, Castle, Mine, Tower, TrojanHorse
		}
		public enum OwnerType {
			None,
			Player,
			Enemy
		}
		#endregion
	}
}