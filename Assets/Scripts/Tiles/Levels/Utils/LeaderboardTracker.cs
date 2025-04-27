using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	public class LeaderboardTracker: MonoBehaviour {
		private readonly Dictionary<Player, int> _players = new Dictionary<Player, int>();
		
		public List<KeyValuePair<Player, int>> GetLeaderboard() {
			return _players.OrderByDescending(p => p.Value).ToList();
		}
		private void LogLeaderboard() {
			var debugText = new StringBuilder();
			debugText.AppendLine("Leaderboard:");
			foreach ((Player player, int value) in _players) {
				debugText.AppendLine($"Player <color={player.Color.ToHex()}>{player.Color.ToHex()}</color> has {value} cells");
			}
			Debug.Log(debugText);
		}
		
		private void OnCellCaptured(CellCapturedEvent gameEvent) {
			if (gameEvent.PreviousOwner != null) {
				if (_players.ContainsKey(gameEvent.PreviousOwner)) {
					_players[gameEvent.PreviousOwner]--;
					
					if (_players[gameEvent.PreviousOwner] <= 0) {
						_players.Remove(gameEvent.PreviousOwner);
					}
				}
			}
			
			if (gameEvent.By != null) {
				if (_players.ContainsKey(gameEvent.By)) {
					_players[gameEvent.By]++;
				} else {
					_players[gameEvent.By] = 1;
				}
			}
		}
		private void OnEnable() {
			EventBus<CellCapturedEvent>.Event += OnCellCaptured;
		}
		private void OnDisable() {
			EventBus<CellCapturedEvent>.Event -= OnCellCaptured;
		}
	}
}