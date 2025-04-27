using System;
using UnityEngine;

namespace Game.Inputs {
	public class ClickInput: MonoBehaviour {
		[SerializeField] private float _maxTime = 0.25f;
		[SerializeField] private float _maxWorldDelta = 0.25f;
		private Camera _camera;
		
		private float _startTime;
		private Vector2 _startPos;

		public event Action Performed;

		private void Awake() {
			_camera = Camera.main;
		}
		private void Update() {
			var worldPos = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
			if (Input.GetMouseButtonDown(0)) {
				_startTime = Time.realtimeSinceStartup;
				_startPos = worldPos;
			}
			if (Input.GetMouseButtonUp(0)) {
				if (CheckDelta(worldPos) && CheckTime(Time.realtimeSinceStartup)) {
					Performed?.Invoke();
				}
			}
			
			bool CheckDelta(Vector2 currentPosition) => Vector2.Distance(_startPos, currentPosition) <= _maxWorldDelta;
			bool CheckTime(float currentTime) => currentTime - _startTime <= _maxTime;
		}
	}
}