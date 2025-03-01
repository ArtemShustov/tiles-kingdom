using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles.Test {
	public class TimeScaleButton: MonoBehaviour {
		[SerializeField] private Button _button;
		[SerializeField] private Image _image;
		[Space]
		[SerializeField] private Sprite _normalSprite;
		[SerializeField] private Sprite _highlightedSprite;
		private bool _active;
		
		private void OnClick() {
			_active = !_active;
			_image.sprite = _active ? _highlightedSprite : _normalSprite;
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