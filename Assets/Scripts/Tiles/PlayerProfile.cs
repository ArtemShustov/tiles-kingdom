using UnityEngine;

namespace Game.Tiles {
	public class PlayerProfile {
		private float _difficulty = 0.5f;
		public float Difficulty {
			set => _difficulty = Mathf.Clamp(value, 0, 1);
			get => _difficulty;
		}
		public DifficultyLevel DifficultyStage => GetDifficultyStage();

		private DifficultyLevel GetDifficultyStage() {
			return _difficulty switch {
				<= 0.25f => DifficultyLevel.Easy,
				>= 0.7f => DifficultyLevel.Hard,
				_ => DifficultyLevel.Normal,
			};
		}
		
		public static PlayerProfile Current { get; private set; }
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void ResetOnLoad() {
			Current = new PlayerProfile();
		}

		public enum DifficultyLevel {
			Easy,
			Normal,
			Hard,
		}
	}
}