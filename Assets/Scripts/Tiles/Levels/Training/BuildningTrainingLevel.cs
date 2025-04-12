using Game.Tiles.Levels.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tiles.Levels.Training {
	[CreateAssetMenu(menuName = "Levels/Training/Building")]
	public class BuildingTrainingLevel: Level {
		[SerializeField] private int _width = 4;
		[SerializeField] private GameObject _hintPrefab;

		public override void Build(LevelRoot root) {
			Instantiate(_hintPrefab);
			root.gameObject.AddComponent<ReloadSceneOnEnd>();
			root.gameObject.AddComponent<RealTimeTicker>();
			var watcher = root.gameObject.AddComponent<SoloLevelWatcher>();
			root.SetBuildingAllowed(true);
			root.UI.MenuLevel = null;
			
			BuildCells(root);
			BuildPlayer(root, new Player(Color.blue, PlayerFlags.Human), watcher);
			BuildEnemy(root, watcher);
			GameBalancer.ResetScore();
		}
		private void BuildCells(LevelRoot root) {
			for (int x = -_width; x < _width; x++) {
				for (int y = -1; y < 2; y++) {
					root.PlaceEmptyCell(new Vector2Int(x, y));
				}
			}
		}
		private void BuildEnemy(LevelRoot root, SoloLevelWatcher watcher) {
			var rightSide = new Vector2Int(_width - 1, 0);
			
			// castle
			var castle = root.AttachCastle(rightSide);
			var enemy = root.AddPlayer(Color.red, castle);
			watcher.AddEnemy(enemy, castle);
			root.AttachMine(rightSide + Vector2Int.up);
			// capture
			for (int x = 0; x < 3; x++) {
				for (int y = -1; y < 2; y++) {
					root.GetCell(rightSide - new Vector2Int(x, y)).Capture(enemy);
				}
			}
		}
		private void BuildPlayer(LevelRoot root, Player player, SoloLevelWatcher watcher) {
			var leftSide = new Vector2Int(-_width, 0);
			
			// castle
			var castle = root.AttachCastle(leftSide);
			castle.Cell.Capture(player);
			watcher.SetPlayer(player, castle);
			root.SetMainPlayer(player, castle);
			root.BindSystems(player, castle);
			// mine
			root.AttachMine(leftSide + Vector2Int.up);
			root.AttachMine(leftSide + Vector2Int.down);
			
			// capture
			for (int x = 0; x < 5; x++) {
				for (int y = -1; y < 2; y++) {
					root.GetCell(leftSide + new Vector2Int(x, y)).Capture(player);
				}
			}
			
			player.StrategyPoints.Add(10);
			player.LogisticsPoints.Add(12);
		}
	}
}