using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game Settings")]
	public class GameSettings: ScriptableObject {
		[SerializeField] private int _targetFps = 60;
		
		public const string DefaultPath = "GameSettings";

		public void Apply() {
			Application.targetFrameRate = _targetFps;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad() {
			var settings = Resources.Load<GameSettings>(DefaultPath) ?? CreateInstance<GameSettings>();
			settings.Apply();
		}
	}
}