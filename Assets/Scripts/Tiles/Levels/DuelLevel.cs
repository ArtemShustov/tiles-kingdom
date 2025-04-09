using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Duel")]
	public class DuelLevel: Level {
		public override void Build(LevelRoot root) {
			var size = 5;
			for (var i = -size; i < size; i++) {
				root.PlaceEmptyCell(new Vector2Int(i, 0));
			}
			
			var player1 = new Player(Color.blue, PlayerFlags.Human);
			var castle1 = root.AttachCastle(new Vector2Int(-size, 0));
			castle1.Cell.Capture(player1);
			root.AttachMine(new Vector2Int(-size + 1, 0)).Cell.Capture(player1);
			root.SetMainPlayer(player1, castle1);
			
			var player2 = new Player(Color.red, PlayerFlags.Human);
			var castle2 = root.AttachCastle(new Vector2Int(size - 1, 0));
			castle2.Cell.Capture(player2);
			root.AttachMine(new Vector2Int(size - 2, 0)).Cell.Capture(player2);
			root.SetSecondPlayer(player2, castle2);

			root.UI.SetTimescaleButton(false);
			root.UI.SetSkipTurnButton(true);
			root.gameObject.AddComponent<DuelTurnMachine>()
				.SetLevel(root)
				.SetFirstPlayer(player1, castle1)
				.SetSecondPlayer(player2, castle2)
				.NextTurn();
			
			player1.StrategyPoints.Add(100);
			player2.StrategyPoints.Add(100);
		}
	}
}