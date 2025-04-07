using System;
using Game.Testing;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Test")]
	public class TestLevel: Level {
		[SerializeField] private int _size = 5;

		public override void Build(LevelRoot root) {
			for (int x = -_size; x < _size; x++) {
				for (int y = -_size; y < _size; y++) {
					root.PlaceEmptyCell(new Vector2Int(x, y));
				}
			}
			BuildPlayer(root);
			BuildEnemy(root);
		}
		private void BuildEnemy(LevelRoot root) {
			var enemyCorner = Vector2Int.one * (_size - 1);
			
			root.AttachTower(enemyCorner + Vector2Int.down * 2);
			root.AttachTower(enemyCorner + Vector2Int.left * 2);
			root.AttachMine(enemyCorner + Vector2Int.down * 1);
			
			var enemyCastle = root.AttachCastle(enemyCorner);
			var enemy = root.AddEnemy(Color.red, enemyCastle);
			
			for (int x = _size - 3; x < _size; x++) {
				for (int y = _size - 3; y < _size; y++) {
					root.GetCell(new Vector2Int(x, y)).Capture(enemy);
				}
			}
			
			root.AddAI(enemy, enemyCastle);

			var bonusPoints = PlayerProfile.Current.Difficulty switch {
				PlayerProfile.DifficultyLevel.Easy => 0,
				PlayerProfile.DifficultyLevel.Normal => 10,
				PlayerProfile.DifficultyLevel.Hard => 50,
				_ => 0
			};
			enemy.StrategyPoints.Add(bonusPoints);

			root.gameObject.AddComponent<TestWin>();
		}
		private void BuildPlayer(LevelRoot root) {
			var playerCorner = Vector2Int.one * -_size;
			
			var playerCastle = root.AttachCastle(playerCorner);
			root.SetPlayer(new Player(Color.blue, PlayerFlags.Human), playerCastle);
			
			root.AttachTower(playerCorner + Vector2Int.one);
			root.AttachMine(playerCorner + Vector2Int.right * 2);
			
			for (int x = -_size; x < -_size + 3; x++) {
				for (int y = -_size; y < -_size + 3; y++) {
					root.GetCell(new Vector2Int(x, y)).Capture(root.Player);
				}
			}
			
			var bonusPoints = PlayerProfile.Current.Difficulty switch {
				PlayerProfile.DifficultyLevel.Easy => 20,
				PlayerProfile.DifficultyLevel.Normal => 10,
				PlayerProfile.DifficultyLevel.Hard => 0,
				_ => 0
			};
			root.Player.StrategyPoints.Add(bonusPoints);
		}
	}
}