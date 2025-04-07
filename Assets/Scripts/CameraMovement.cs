using Game.Inputs;
using UnityEngine;

namespace Game {
	public class CameraMovement: MonoBehaviour {
		[SerializeField] private DragInput _input;
		[SerializeField] private Camera _camera;

		private Vector2 _startMousePosition;
		
		private void Awake() {
			DisableInput();
		}
		
		public void EnableInput() => _input.enabled = true;
		public void DisableInput() => _input.enabled = false;
		
		private void OnInputStarted(Vector2 worldPosition) {
			_startMousePosition = worldPosition;
		}
		private void OnInputPerformed(Vector2 worldPosition) {
			var delta = _startMousePosition - worldPosition;
			_camera.transform.position += (Vector3)delta;
		}
		private void OnEnable() {
			_input.Started += OnInputStarted;
			_input.Performed += OnInputPerformed;
		}
		private void OnDisable() {
			_input.Performed -= OnInputPerformed;
		}
	}
}