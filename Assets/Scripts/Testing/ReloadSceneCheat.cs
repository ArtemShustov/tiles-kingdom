using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Testing {
	public class ReloadSceneCheat: MonoBehaviour {
		[SerializeField] private KeyCode _key = KeyCode.F5;

		private void Update() {
			if (Input.GetKeyDown(_key)) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
		
		#if UNITY_EDITOR || DEBUG || DEVELOPMENT_BUILD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeOnLoad() {
			var instance = new GameObject("ReloadSceneCheat", typeof(ReloadSceneCheat));
			DontDestroyOnLoad(instance);
		}
		#endif
	}
}