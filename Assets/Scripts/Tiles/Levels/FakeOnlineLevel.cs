using System.Collections.Generic;
using System.Linq;
using Core;
using Game.Tiles.Buildings;
using Game.Tiles.Levels.Utils;
using Game.Tiles.UI;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Fake online")]
	public class FakeOnlineLevel: Level {
		[SerializeField] private Vector2 _turnSkip;
		[SerializeField] private int _radius = 10;
		[SerializeField] private int _enemiesCount = 10;
		[SerializeField] private int _minesCount = 10;
		
		public override void Build(LevelRoot root) {
			ConfigureLevel(root);
			PlaceAllCells(root);

			var watcher = root.gameObject.AddComponent<SoloLevelWatcher>();
			var cells = root.Grid.Cells.Values.ToArray();
			var player = PlaceHuman(cells, root, watcher);
			var bots = PlaceAllBots(cells, root, watcher);
			PlaceAllMines(cells, root);
			
			root.gameObject.AddComponent<CheatBonus>().Init(player, bots);
		}
		
		private void ConfigureLevel(LevelRoot root) {
			GameBalancer.ResetScore();
			PlayerProfile.Current.Difficulty = 1f;
			
			root.gameObject.AddComponent<StealEnemyCells>().SetRoot(root);
			root.gameObject.AddComponent<RealTimeTicker>();
			root.gameObject.AddComponent<ReloadSceneOnEnd>().SetRoot(root);
			root.UI.Leaderboard.Tracker = root.gameObject.AddComponent<LeaderboardTracker>();
			root.SetLeaderboardAllowed(true);
			root.SetBuildingAllowed(false);
			root.SetTimescaleAllowed(false);
			root.SetFreeCameraAllowed(true);
		}
		private void PlaceAllCells(LevelRoot root) {
			var center = Vector2Int.zero;
			for (int x = -_radius; x <= _radius; x++) {
				for (int y = -_radius; y <= _radius; y++) {
					var pos = new Vector2Int(x, y);
					if (Vector2Int.Distance(center, pos) <= _radius) {
						root.PlaceEmptyCell(pos);
					}
				}
			}
		}
		private Player[] PlaceAllBots(Cell[] cells, LevelRoot root, SoloLevelWatcher watcher) {
			var bots = new List<Player>();
			
			for (int i = 0; i < _enemiesCount; i++) {
				var enemyCell = cells.GetRandom();
				if (CanPlacePlayer(root, enemyCell)) {
					var enemy = new Player(Game.Utils.GetRandomNiceColor(), PlayerFlags.AI | PlayerFlags.Cheating);
					var enemyCastle = PlacePlayer(root, enemyCell, enemy);
					watcher.AddEnemy(enemy, enemyCastle);
					root.AddAI(enemy, enemyCastle).SetTurnSkipChance(Mathf.RoundToInt(_turnSkip.RandomBetween()));
					bots.Add(enemy);
				} else {
					Debug.Log($"Can't place enemy on {enemyCell.Position}");
					i--;
				}
			}
			
			return bots.ToArray();
		}
		private void PlaceAllMines(Cell[] cells, LevelRoot root) {
			for (int i = 0; i < _minesCount; i++) {
				var cell = cells.GetRandom();
				if (cell.Building.Value == null && cell.Owner.Value == null) {
					root.AttachMine(cell.Position);
				} else {
					Debug.Log($"Can't place mine on {cell.Position}");
				}
			}
		}
		
		private bool CanPlacePlayer(LevelRoot root, Cell center) {
			if (center.Building.Value != null || center.Owner.Value != null) {
				return false;
			}
			return root.Grid.GetNeighbours(center.Position).All(Filter);

			bool Filter(Cell cell) {
				return cell.Building.Value || cell.Owner.Value == null;
			}
		}
		private Castle PlacePlayer(LevelRoot root, Cell center, Player player) {
			center.Capture(player);
			var castle = root.AttachCastle(center.Position);
			var neighbours = root.Grid.GetNeighbours(center.Position).ToArray();
			foreach (var neighbour in neighbours) {
				neighbour.Capture(player);
			}
			var mineCell = neighbours.GetRandom();
			if (!mineCell.Building.Value) {
				root.AttachMine(mineCell.Position);
			}
			return castle;
		}
		private Player PlaceHuman(Cell[] cells, LevelRoot root, SoloLevelWatcher watcher) {
			var cell = cells.GetRandom();
			var player = new Player(Color.blue, PlayerFlags.Human);
			var playerCastle = PlacePlayer(root, cell, player);
			root.SetCameraPosition(cell.transform.position);
			watcher.SetPlayer(player, playerCastle);
			root.BindSystems(player, playerCastle);
			root.SetMainPlayer(player, playerCastle);
			player.StrategyPoints.Add(3);
			return player;
		}
	}
}