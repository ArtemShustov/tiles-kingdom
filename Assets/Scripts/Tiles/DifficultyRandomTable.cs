using System;
using Core;
using UnityEngine;

namespace Game.Tiles {
	[Serializable]
	public class DifficultyRandomTable {
		[SerializeField] private Vector2 _easyRange;
		[SerializeField] private Vector2 _normalRange;
		[SerializeField] private Vector2 _hardRange;

		public float Get() => PlayerProfile.Current.DifficultyStage switch {
			PlayerProfile.DifficultyLevel.Easy => _easyRange.RandomBetween(),
			PlayerProfile.DifficultyLevel.Normal => _normalRange.RandomBetween(),
			PlayerProfile.DifficultyLevel.Hard => _hardRange.RandomBetween(),
			_ => throw new ArgumentOutOfRangeException()
		};

		public DifficultyRandomTable(Vector2 easyRange, Vector2 normalRange, Vector2 hardRange) {
			_easyRange = easyRange;
			_normalRange = normalRange;
			_hardRange = hardRange;
		}
	}
}