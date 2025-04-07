using UnityEngine;

namespace Game.Inputs {
	public class DragInput: MonoBehaviour {
		[SerializeField] private float _minWorldDelta = 0.25f;
		private Camera _camera;
		private Vector2 _startPosition;
		private bool _active;

		public event DragInputAction Started;
		public event DragInputAction Performed;
		public event DragInputAction Ended;
		
		private void Awake() {
			_camera = Camera.main;
		}
		private void Update() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			if (Input.GetMouseButtonDown(0)) {
				_startPosition = worldPos;
				_active = false;
			}
			if (Input.GetMouseButton(0)) {
				if (!_active && CheckDelta(worldPos)) {
					_active = true;
					Started?.Invoke(worldPos);
				}
				if (_active) {
					Performed?.Invoke(worldPos);
				} 
			}
			if (_active && Input.GetMouseButtonUp(0)) {
				_active = false;
				Ended?.Invoke(worldPos);
			}
			
			bool CheckDelta(Vector2 currentPosition) => Vector2.Distance(_startPosition, currentPosition) >= _minWorldDelta;
		}
		
		public delegate void DragInputAction(Vector2 worldPosition); 
	}
}