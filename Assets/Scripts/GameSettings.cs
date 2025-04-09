using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game Settings")]
	public class GameSettings: ScriptableObject {
		[SerializeField] private int _targetFps = 60;
		
		public void Apply() {
			Application.targetFrameRate = _targetFps;
		}

		public const string DefaultPath = "GameSettings";
		public static GameSettings Current { get; private set; }
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad() {
			Current = Resources.Load<GameSettings>(DefaultPath) ?? CreateInstance<GameSettings>();
			Current.Apply();
		}
	}
}