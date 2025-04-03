using System;
using UnityEngine;

namespace Game.Inputs {
	public class HoldInput: MonoBehaviour {
		[SerializeField] private bool _useAltButton = true;
		[SerializeField] private float _minTime = 0.5f;
		[SerializeField] private float _maxDelta = 10;
		
		private float _startTime;
		private Vector2 _startPos;

		public event Action Performed;

		private void Update() {
			if (Input.GetMouseButtonDown(0)) {
				_startTime = Time.realtimeSinceStartup;
				_startPos = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp(0)) {
				OnMouseUp();
			}
			if (_useAltButton && Input.GetMouseButtonUp(1)) {
				Performed?.Invoke();
			}
		}

		private void OnMouseUp() {
			var valid = Time.realtimeSinceStartup - _startTime >= _minTime 
			            && Vector2.Distance(_startPos, Input.mousePosition) <= _maxDelta;
			if (valid) {
				Performed?.Invoke();
			}
		}
	}
}