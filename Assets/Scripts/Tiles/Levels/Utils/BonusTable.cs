using System;
using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	[Serializable]
	public struct BonusTable {
		[SerializeField] private int _easy;
		[SerializeField] private int _normal;
		[SerializeField] private int _hard;
		
		public BonusTable(int easy, int normal, int hard) {
			_easy = easy;
			_normal = normal;
			_hard = hard;
		}

		public int GetFor(PlayerProfile.DifficultyLevel difficulty) {
			return difficulty switch {
				PlayerProfile.DifficultyLevel.Easy => _easy,
				PlayerProfile.DifficultyLevel.Normal => _normal,
				PlayerProfile.DifficultyLevel.Hard => _hard,
				_ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
			};
		}
	}
}