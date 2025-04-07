using Game.Tiles.Levels;
using UnityEngine;

namespace Game.Tiles {
	public class LevelBoot: MonoBehaviour {
		[SerializeField] private Level _level;
		[Space]
		[SerializeField] private LevelRoot _root;

		private void Awake() {
			_level.Build(_root);
		}

		public void ClearPlayerPrefs() {
			PlayerPrefs.DeleteAll(); 
		}
	}
}