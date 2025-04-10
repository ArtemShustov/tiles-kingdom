using System.Linq;
using Game.Tiles.Buildings;
using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Duel")]
	public class DuelLevel: SerializedLevel {
		[SerializeField] private int _startStrategy = 3;
		[SerializeField] private int _startLogistics = 0;
		
		protected override void ConfigureLevel(LevelRoot root) {
			root.UI.SetTimescaleButton(false);
			root.UI.SetSkipTurnButton(true);
			root.UI.MainPlayer.Background = true;
			root.UI.SecondPlayer.Background = true;
		}
		protected override void PlacePlayers(LevelRoot root) {
			var player1 = new Player(Players[0], PlayerFlags.Human);
			var castle1 = CaptureAll(root, 0, player1);
			root.SetMainPlayer(player1, castle1);
			
			var player2 = new Player(Players[1], PlayerFlags.Human);
			var castle2 = CaptureAll(root, 1, player2);
			root.SetSecondPlayer(player2, castle2);
			
			root.gameObject.AddComponent<DuelTurnMachine>()
				.SetLevel(root)
				.SetFirstPlayer(player1, castle1)
				.SetSecondPlayer(player2, castle2)
				.NextTurn();
			
			player1.StrategyPoints.Add(_startStrategy);
			player1.LogisticsPoints.Add(_startLogistics);
			player2.StrategyPoints.Add(_startStrategy);
			player2.LogisticsPoints.Add(_startLogistics);
		}
		private Castle CaptureAll(LevelRoot root, int owner, Player player) {
			var cells = Cells
				.Where(c => c.Owner == owner)
				.Select(c => root.GetCell(c.Position))
				.Where(c => c != null)
				.ToArray();
			foreach (var cell in cells) {
				cell.Capture(player);
			}
			return (Castle)cells.FirstOrDefault(c => c.Building.Value is Castle)?.Building.Value;
		}
	}
}