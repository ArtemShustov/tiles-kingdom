using Game.Tiles.Levels.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tiles.Levels.Training {
	[CreateAssetMenu(menuName = "Levels/Training/Third")]
	public class ThirdTrainingLevel: Level {
		[SerializeField] private int _width = 4;
		[SerializeField] private GameObject _hintPrefab;

		public override void Build(LevelRoot root) {
			var hint = Instantiate(_hintPrefab);
			root.gameObject.AddComponent<ReloadSceneOnEnd>();
			
			for (int x = -_width; x < _width; x++) {
				for (int y = -1; y < 2; y++) {
					root.PlaceEmptyCell(new Vector2Int(x, y));
				}
			}
			BuildPlayer(root);
			BuildEnemy(root);
			GameBalancer.ResetScore();
		}
		private void BuildEnemy(LevelRoot root) {
			var rightSide = new Vector2Int(_width - 1, 0);
			
			// castle
			var castle = root.AttachCastle(rightSide);
			var enemy = root.AddEnemy(Color.red, castle);
			// tower
			root.AttachTower(rightSide - Vector2Int.right * 2 + Vector2Int.up);
			// capture
			for (int x = 0; x < 4; x++) {
				for (int y = -1; y < 2; y++) {
					root.GetCell(rightSide - new Vector2Int(x, y)).Capture(enemy);
				}
			}
		}
		private void BuildPlayer(LevelRoot root) {
			var leftSide = new Vector2Int(-_width, 0);
			
			// castle
			var castle = root.AttachCastle(leftSide);
			castle.Cell.Capture(root.Player);
			root.SetPlayerCastle(castle);
			// mine
			root.AttachMine(leftSide + Vector2Int.up);
			root.AttachMine(leftSide + Vector2Int.down);
			
			// capture
			for (int x = 0; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					root.GetCell(leftSide + new Vector2Int(x, y)).Capture(root.Player);
				}
			}
			
			root.Player.StrategyPoints.Add(10);
		}
	}
}