using UnityEngine;

namespace Game {
	public class CameraScaler: MonoBehaviour {
		[SerializeField] private Vector2 _defaultResolution = new Vector2(1280, 720);
		[SerializeField] private float _minVerticalSize = 0;

		[SerializeField] private Camera _camera;

		private float _initialSize;
		private float _targetAspect;

		private void Start() {
			_initialSize = _camera.orthographicSize;
			_targetAspect = _defaultResolution.x / _defaultResolution.y;
		}
		private void Update() {
			if (_camera.orthographic) {
				var newSize = _initialSize * (_targetAspect / _camera.aspect);
				_camera.orthographicSize = Mathf.Clamp(newSize, _minVerticalSize, newSize);
			}
		}
	}
}