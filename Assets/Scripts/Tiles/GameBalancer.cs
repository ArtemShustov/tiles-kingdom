using Core.Events;
using Game.Tiles.Events;
using UnityEngine;
using DifficultyLevel = Game.Tiles.PlayerProfile.DifficultyLevel;

namespace Game.Tiles {
	public class GameBalancer: MonoBehaviour {
		[SerializeField] private KeyCode _changeDifficultyKey = KeyCode.D;
		[SerializeField] private int _lowScore = -3;
		[SerializeField] private int _highScore = 3;
		private static int _score = 0;

		#if DEBUG || UNITY_EDITOR || DEVELOPMENT_BUILD
		private void Update() {
			if (Input.GetKeyDown(_changeDifficultyKey)) {
				PlayerProfile.Current.Difficulty = PlayerProfile.Current.Difficulty switch {
					DifficultyLevel.Easy => DifficultyLevel.Normal,
					DifficultyLevel.Normal => DifficultyLevel.Hard,
					DifficultyLevel.Hard => DifficultyLevel.Easy,
					_ => DifficultyLevel.Normal
				};
				Debug.Log($"Difficulty changed to {PlayerProfile.Current.Difficulty}");
			}
		}
		#endif
		
		private void UpdateDifficulty() {
			if (_score <= _lowScore) {
				PlayerProfile.Current.Difficulty = DifficultyLevel.Easy;
			} else if (_score >= _highScore) {
				PlayerProfile.Current.Difficulty = DifficultyLevel.Hard;
			} else if (_score == 0) {
				PlayerProfile.Current.Difficulty = DifficultyLevel.Normal;
			} else {
				return;
			}
			Debug.Log($"Difficulty changed to {PlayerProfile.Current.Difficulty}");
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
			_score -= 1;
			UpdateDifficulty();
		}
		private void OnWin(PlayerWinEvent gameEvent) {
			_score += 1;
			UpdateDifficulty();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void ResetScore() {
			_score = 0;
		}
	}
}