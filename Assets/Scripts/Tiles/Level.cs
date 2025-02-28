using UnityEngine;

namespace Game.Tiles {
	public abstract class Level: ScriptableObject {
		public abstract void Build(LevelRoot root);
	}
}