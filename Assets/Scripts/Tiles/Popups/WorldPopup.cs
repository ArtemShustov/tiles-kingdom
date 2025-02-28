using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Tiles.Popups {
	public class WorldPopup: MonoBehaviour {
		[SerializeField] private TMP_Text _label;
		[SerializeField] private float _duration = 1;
		[SerializeField] private float _speed = 0.5f;

		private void Start() {
			StartCoroutine(Animate());
		}
		public void Setup(string text, Color color) {
			_label.text = text;
			_label.color = color;
		}

		private IEnumerator Animate() {
			var time = 0f;
			while (time < _duration) {
				yield return new WaitForEndOfFrame();
				time += Time.deltaTime;
				transform.position += Vector3.up * (_speed * Time.deltaTime);
			}
			Destroy(gameObject);
		}
	}
}