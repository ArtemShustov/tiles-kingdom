using System;
using UnityEngine;

namespace Game.Inputs {
	public class ClickInput: MonoBehaviour {
		[SerializeField] private float _maxTime = 0.25f;
		
		private float _startTime;

		public event Action Performed;
		
		private void Update() {
			if (Input.GetMouseButtonDown(0)) {
				_startTime = Time.realtimeSinceStartup;
			}
			if (Input.GetMouseButtonUp(0)) {
				if (Time.realtimeSinceStartup - _startTime <= _maxTime) {
					Performed?.Invoke();
				}
			}
		}
	}
}