using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels.Training {
	[CreateAssetMenu(menuName = "Levels/Training/Capture")]
	public class CaptureTrainingLevel: Level {
		[SerializeField] private int _size = 4;
		[SerializeField] private GameObject _hintPrefab;

		public override void Build(LevelRoot root) {
			Instantiate(_hintPrefab);
			root.gameObject.AddComponent<ReloadSceneOnEnd>();
			root.gameObject.AddComponent<RealTimeTicker>();
			var watcher = root.gameObject.AddComponent<SoloLevelWatcher>();
			root.SetBuildingAllowed(false);
			root.UI.MenuLevel = null;
			
			BuildCells(root);
			BuildPlayer(root, new Player(Color.blue, PlayerFlags.Human), watcher);
			BuildEnemy(root, watcher);
		}
		private void BuildCells(LevelRoot root) {
			for (int x = -_size; x < _size; x++) {
				root.PlaceEmptyCell(new Vector2Int(x, 0));
			}
		}
		private void BuildEnemy(LevelRoot root, SoloLevelWatcher watcher) {
			var rightSide = new Vector2Int(_size - 1, 0);

			var castle = root.AttachCastle(rightSide);
			var enemy = root.AddPlayer(Color.red, castle);
			castle.Cell.Capture(enemy);
			watcher.AddEnemy(enemy, castle);
			root.GetCell(rightSide + Vector2Int.left).Capture(enemy);
			root.GetCell(rightSide + Vector2Int.left * 2).Capture(enemy);
		}
		private void BuildPlayer(LevelRoot root, Player player, SoloLevelWatcher watcher) {
			var leftSide = new Vector2Int(-_size, 0);
			
			var castle = root.AttachCastle(leftSide);
			castle.Cell.Capture(player);
			watcher.SetPlayer(player, castle);
			root.SetMainPlayer(player, castle);
			root.BindSystems(player, castle);
			root.GetCell(leftSide + Vector2Int.right).Capture(player);
			
			player.StrategyPoints.Add(16);
		}
	}
}