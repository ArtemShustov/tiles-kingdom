using Game.Tiles.Levels.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tiles.Levels.Training {
	[CreateAssetMenu(menuName = "Levels/Training/Second")]
	public class MinesTrainingLevel: Level {
		[SerializeField] private int _width = 4;
		[SerializeField] private GameObject _hintPrefab;

		public override void Build(LevelRoot root) {
			var hint = Instantiate(_hintPrefab);
			root.gameObject.AddComponent<ReloadSceneOnEnd>();
			root.SetBuildingAllowed(false);
			
			for (int x = -_width; x < _width; x++) {
				for (int y = -1; y < 2; y++) {
					root.PlaceEmptyCell(new Vector2Int(x, y));
				}
			}
			BuildPlayer(root);
			BuildEnemy(root);
		}
		private void BuildEnemy(LevelRoot root) {
			var rightSide = new Vector2Int(_width - 1, 0);
			
			var castle = root.AttachCastle(rightSide);
			var enemy = root.AddEnemy(Color.red, castle);
			// capture
			for (int x = 0; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					root.GetCell(rightSide - new Vector2Int(x, y)).Capture(enemy);
				}
			}
		}
		private void BuildPlayer(LevelRoot root) {
			var leftSide = new Vector2Int(-_width, 0);
			
			// castle
			var castle = root.AttachCastle(leftSide);
			root.SetPlayer(new Player(Color.blue, PlayerFlags.Human), castle);
			castle.Cell.Capture(root.Player);
			// mine
			for (int y = -1; y < 2; y++) {
				root.AttachMine(leftSide + new Vector2Int(2, y));
			}

			// capture
			for (int x = 0; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					root.GetCell(leftSide + new Vector2Int(x, y)).Capture(root.Player);
				}
			}
			
			root.Player.StrategyPoints.Add(5);
		}
	}
}