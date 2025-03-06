using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
	public class TimeScaleButton: MonoBehaviour {
		[SerializeField] private Button _button;
		[Space]
		[SerializeField] private GameObject _normal;
		[SerializeField] private GameObject _highlighted;
		private bool _active;

		private void Awake() {
			_normal.SetActive(!_active);
			_highlighted.SetActive(_active);
		}
		private void OnClick() {
			_active = !_active;
			_normal.SetActive(!_active);
			_highlighted.SetActive(_active);
			Time.timeScale = _active ? 4 : 1;
		}
		private void OnEnable() {
			_button.onClick.AddListener(OnClick);
		}
		private void OnDisable() {
			_button.onClick.RemoveListener(OnClick);
		}
		private void OnDestroy() {
			Time.timeScale = 1.0f;
		}
	}
}