using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels.Training {
	[CreateAssetMenu(menuName = "Levels/Training/Capture")]
	public class CaptureTrainingLevel: Level {
		[SerializeField] private int _size = 4;
		[SerializeField] private GameObject _hintPrefab;

		public override void Build(LevelRoot root) {
			var hint = Instantiate(_hintPrefab);
			root.gameObject.AddComponent<ReloadSceneOnEnd>();
			root.SetBuildingAllowed(false);
			
			for (int x = -_size; x < _size; x++) {
				root.PlaceEmptyCell(new Vector2Int(x, 0));
			}
			BuildPlayer(root);
			BuildEnemy(root);
		}
		private void BuildEnemy(LevelRoot root) {
			var rightSide = new Vector2Int(_size - 1, 0);

			var castle = root.AttachCastle(rightSide);
			var enemy = root.AddEnemy(Color.red, castle);
			castle.Cell.Capture(enemy);
			root.GetCell(rightSide + Vector2Int.left).Capture(enemy);
			root.GetCell(rightSide + Vector2Int.left * 2).Capture(enemy);
		}
		private void BuildPlayer(LevelRoot root) {
			var leftSide = new Vector2Int(-_size, 0);
			
			var castle = root.AttachCastle(leftSide);
			root.SetPlayer(new Player(Color.blue, PlayerFlags.Human), castle);
			castle.Cell.Capture(root.Player);
			root.GetCell(leftSide + Vector2Int.right).Capture(root.Player);
			
			root.Player.StrategyPoints.Add(16);
		}
	}
}