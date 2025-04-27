using Core;
using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	public class CheatBonus: MonoBehaviour {
		[SerializeField] private Vector2Int _bonus = new Vector2Int(3, 10);
		[SerializeField] private float _delay = 4;
		private Player _player;
		private Player[] _bots;

		private float _timer;

		public void Init(Player player, Player[] bots) {
			_player = player;
			_bots = bots;
		}
		private void Update() {
			if (_player == null) {
				return;
			}
			_timer += Time.deltaTime;
			if (_timer <= _delay) {
				return;
			}
			
			_timer = 0;
			if (_player.LogisticsPoints.Value >= 70) {
				foreach (var bot in _bots) {
					bot.StrategyPoints.Add(_bonus.RandomBetween() * 2);
				}
				Debug.Log("CHEAT BONUS 70");
			}
			if (_player.LogisticsPoints.Value >= 30) {
				foreach (var bot in _bots) {
					bot.StrategyPoints.Add(_bonus.RandomBetween());
				}
				Debug.Log("CHEAT BONUS 30");
			}
		}
	}
}