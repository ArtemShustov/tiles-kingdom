using UnityEngine;

namespace Game.Tiles.Test {
	public class TimeCheat: MonoBehaviour {
		[SerializeField] private KeyCode _key = KeyCode.T;
		[SerializeField] private float[] _modes = { 0, 1, 2, 10, 50 };
		private int _currentMode = 1;
		
		private void Awake() {
			Time.timeScale = _modes[_currentMode];
		}
		private void Update() {
			if (Input.GetKeyDown(_key)) {
				_currentMode = (_currentMode + 1) >= _modes.Length? 0 : _currentMode + 1;
				Time.timeScale = _modes[_currentMode];
				Debug.Log($"Set time scale to {Time.timeScale}");
			}
		}

		#if UNITY_EDITOR || DEBUG || DEVELOPMENT_BUILD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeOnLoad() {
			var instance = new GameObject("TimeCheat", typeof(TimeCheat));
			DontDestroyOnLoad(instance);
		}
		#endif
	}
}