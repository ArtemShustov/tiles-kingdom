using UnityEngine;
using System.Collections.Generic;
using Game.Tiles.Levels.Utils;

namespace Game.Tiles.UI {
	public class Leaderboard : MonoBehaviour {
		[SerializeField] private Transform _container;
		[SerializeField] private LeaderboardEntry _prefab;
		public LeaderboardTracker Tracker { get; set; }

		private readonly List<LeaderboardEntry> _currentEntries = new List<LeaderboardEntry>();

		private void Awake() {
			SetEnabled(false);
		}
		private void Update() {
			if (Tracker != null) {
				UpdateLeaderboard();
			}
		}

		public void SetEnabled(bool active) {
			gameObject.SetActive(active);
		}
		private void UpdateLeaderboard() {
			List<KeyValuePair<Player, int>> leaderboard = Tracker.GetLeaderboard();
			
			int leaderboardCount = Mathf.Min(leaderboard.Count, 5);
			
			for (int i = 0; i < leaderboardCount; i++) {
				if (i < _currentEntries.Count) {
					_currentEntries[i].Bind(leaderboard[i].Key, leaderboard[i].Value, i + 1);
				} else {
					var entry = Instantiate(_prefab, _container);
					entry.Bind(leaderboard[i].Key, leaderboard[i].Value, i + 1);
					_currentEntries.Add(entry);
				}
			}
			
			for (int i = leaderboardCount; i < _currentEntries.Count; i++) {
				Destroy(_currentEntries[i].gameObject);
			}
			
			_currentEntries.RemoveRange(leaderboardCount, _currentEntries.Count - leaderboardCount);
		}
	}
}