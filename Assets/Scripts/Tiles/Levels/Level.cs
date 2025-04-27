using UnityEngine;

namespace Game.Tiles.Levels {
	public abstract class Level: ScriptableObject {
		public abstract void Build(LevelRoot root);
	}
}