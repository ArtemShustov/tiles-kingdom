using Core;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Random")]
	public class RandomLevel: Level {
		[SerializeField] private Level[] _levels;
		
		public override void Build(LevelRoot root) {
			_levels.GetRandom().Build(root);
		}
	}
}