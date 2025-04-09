using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	public class DuelTurnMachine: MonoBehaviour {
		private Player _firstPlayer;
		private Castle _firstCastle;
		private Player _secondPlayer;
		private Castle _secondCastle;
		private LevelRoot _level;
		
		private bool _firstPlayerTurn;
		
		public DuelTurnMachine SetLevel(LevelRoot level) {
			_level = level;
			return this;
		}
		public DuelTurnMachine SetFirstPlayer(Player player, Castle castle) {
			_firstPlayer = player;
			_firstCastle = castle;
			return this;
		}
		public DuelTurnMachine SetSecondPlayer(Player player, Castle castle) {
			_secondPlayer = player;
			_secondCastle = castle;
			return this;
		}

		public void NextTurn() {
			_firstPlayerTurn = !_firstPlayerTurn;
			LevelRoot.TickAll(10);
			if (_firstPlayerTurn) {
				_level.BindSystems(_firstPlayer, _firstCastle);
				_level.UI.MainPlayer.ShowTurnLabelAsync().Forget();
				_level.UI.SecondPlayer.HideTurnLabelAsync().Forget();
			} else {
				_level.BindSystems(_secondPlayer, _secondCastle);
				_level.UI.MainPlayer.HideTurnLabelAsync().Forget();
				_level.UI.SecondPlayer.ShowTurnLabelAsync().Forget();
			}
		}

		private void OnPlayerActed(PlayerActedEvent gameEvent) {
			NextTurn();
		}
		private void OnEnable() {
			EventBus<PlayerActedEvent>.Event += OnPlayerActed;
		}
		private void OnDisable() {
			EventBus<PlayerActedEvent>.Event -= OnPlayerActed;
		}
	}
}