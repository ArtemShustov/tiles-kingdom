using UnityEngine;

namespace Game.Tiles.Test {
	public class CaptureCostView: MonoBehaviour {
		[SerializeField] private int _fontSize = 32;
		private Camera _camera;
		private PlayGrid _grid;
		private Player _player;
		
		private void Awake() {
			_camera = Camera.main;
			_grid = FindAnyObjectByType<PlayGrid>();
			_player = FindAnyObjectByType<LevelRoot>()?.Player;
		}
		private void OnGUI() {
			var style = new GUIStyle();
			style.fontSize = _fontSize;
			style.normal.textColor = Color.white;
			style.fontStyle = FontStyle.Bold;

			if (!_grid || !_camera || _player == null) {
				_grid = FindAnyObjectByType<PlayGrid>();
				_player = FindAnyObjectByType<LevelRoot>()?.Player;
				_camera = Camera.main;
				if (!_grid || _player == null) {
					return;
				}
			}
			var position = GetCellUnderMouse();
			if (_grid.TryGetCell(position, out var cell)) {
				GUILayout.Label($"\n\nCost: {cell.GetCaptureCostFor(_player)}", style);
			}
		}

		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _grid.WorldToCell(worldPos);
			return cellPos;
		}

		#if UNITY_EDITOR || DEBUG || DEVELOPMENT_BUILD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeOnLoad() {
			var instance = new GameObject("CaptureCostView", typeof(CaptureCostView));
			DontDestroyOnLoad(instance);
		}
		#endif
	}
}