using Game.Tiles.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game.Tiles {
	public class LevelBoot: MonoBehaviour {
		[FormerlySerializedAs("_level")]
		[SerializeField] private Level _defaultLevel;
		[SerializeField] private Level _menuLevel;
		[Space]
		[SerializeField] private LevelRoot _root;

		public static Level LevelToLoad { get; set; }
		
		private void Awake() {
			_root.UI.MenuLevel = _menuLevel;
			if (LevelToLoad == null) {
				Debug.Log("Loaded default level");
				LevelToLoad = _defaultLevel;
			}
			LevelToLoad.Build(_root);
		}

		public void ClearPlayerPrefs() {
			PlayerPrefs.DeleteAll(); 
		}

		public static void Restart() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void ClearOnLoad() {
			LevelToLoad = null;
		}
	}
}