using System.Collections.Generic;
using System.Linq;
using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	public class SoloLevelWatcher: MonoBehaviour {
		private readonly List<Castle> _enemiesCastles = new List<Castle>();
		private Castle _playerCastle;
		private Player _player;

		public void AddEnemy(Player enemy, Castle castle) {
			_enemiesCastles.Add(castle);
			castle.Captured += OnEnemyCaptured;
		}
		public void SetPlayer(Player player, Castle castle) {
			_playerCastle = castle;
			_playerCastle.Captured += OnPlayerCaptured;
			_player = player;
		}
		
		private void OnPlayerCaptured(Player by) {
			if (by == _player) {
				return;
			}
			EventBus<PlayerLoseEvent>.Raise(new PlayerLoseEvent());
			Debug.Log($"LOSE");
		}
		private void OnEnemyCaptured(Player by) {
			if (by != _player) {
				return;
			}
			var isAllCaptured = _enemiesCastles.All(castle => castle.Cell.Owner.Value == _player);
			if (isAllCaptured) {
				EventBus<PlayerWinEvent>.Raise(new PlayerWinEvent());
				Debug.Log($"WIN");
			}
		}
	}
}