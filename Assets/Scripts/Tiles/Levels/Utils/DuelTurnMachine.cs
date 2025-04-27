using System.Threading.Tasks;
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
			_firstCastle.Captured += OnCastleCaptured;
			return this;
		}
		public DuelTurnMachine SetSecondPlayer(Player player, Castle castle) {
			_secondPlayer = player;
			_secondCastle = castle;
			_secondCastle.Captured += OnCastleCaptured;
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
		private void OnCastleCaptured(Player by) {
			EventBus<PlayerWinEvent>.Raise(new PlayerWinEvent());
			Captured()?.Forget();

			async Task Captured() {
				await _level.UI.WinPanel.ShowAndHideAsync();
				LevelBoot.Restart();
			}
		}
		private void OnEnable() {
			EventBus<PlayerActedEvent>.Event += OnPlayerActed;
		}
		private void OnDisable() {
			EventBus<PlayerActedEvent>.Event -= OnPlayerActed;
		}
	}
}