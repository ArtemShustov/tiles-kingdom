using Core.Events;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles {
	public class GameBalancer: MonoBehaviour {
		private void UpdateDifficulty() {
			Debug.Log($"Difficulty changed to {PlayerProfile.Current.Difficulty} ({PlayerProfile.Current.DifficultyStage})");
		}
		
		private void OnEnable() {
			EventBus<PlayerWinEvent>.Event += OnWin;
			EventBus<PlayerLoseEvent>.Event += OnLose;
		}
		private void OnDisable() {
			EventBus<PlayerWinEvent>.Event -= OnWin;
			EventBus<PlayerLoseEvent>.Event -= OnLose;
		}
		private void OnLose(PlayerLoseEvent gameEvent) {
			PlayerProfile.Current.Difficulty -= 0.2f;
			UpdateDifficulty();
		}
		private void OnWin(PlayerWinEvent gameEvent) {
			PlayerProfile.Current.Difficulty += 0.1f;
			if (PlayerProfile.Current.Difficulty >= 0.9f) {
				PlayerProfile.Current.Difficulty = 0.2f;
			}
			UpdateDifficulty();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void ResetScore() {
			if (PlayerProfile.Current != null) {
				PlayerProfile.Current.Difficulty = 0.5f;
				Debug.Log($"Reset difficulty to {PlayerProfile.Current.Difficulty} ({PlayerProfile.Current.DifficultyStage})");
			}
		}
	}
}