using UnityEngine;

namespace Game.Tiles {
	public class PlayerProfile {
		public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Normal;
		
		public static PlayerProfile Current { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void ResetOnLoad() {
			Current = new PlayerProfile();
		}
		
		public enum DifficultyLevel {
			Easy, Normal, Hard
		}
	}
}