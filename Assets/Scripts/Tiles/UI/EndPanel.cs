using System.Threading.Tasks;
using UnityEngine;

namespace Game.Tiles.UI {
	public class EndPanel : MonoBehaviour {
		[Header("Settings")]
		[SerializeField] private float _showDuration = 0.5f;
		[SerializeField] private float _waitDuration = 1f;
		[SerializeField] private float _hideDuration = 0.5f;
		[Header("Components")]
		[SerializeField] private GameObject _container;
		[SerializeField] private RectTransform _element;
		[SerializeField] private CanvasGroup _group;

		private void Awake() {
			_container.gameObject.SetActive(false);
			_group.alpha = 0f;
		}
		private void Update() {
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				ShowAndHideAsync()?.Forget();
			}
		}

		public async Task ShowAndHideAsync() {
			_container.SetActive(true);
			await ShowAnimate();
			await Awaitable.WaitForSecondsAsync(_waitDuration);
			await HideAnimate();
			_container.SetActive(false);
		}

		private async Task ShowAnimate() {
			var t = 0f;
			while (t < _showDuration) {
				t += Time.deltaTime;
				await Awaitable.NextFrameAsync();

				var position = _element.anchoredPosition;
				position.y = Mathf.Lerp(Screen.height / -2f - _element.rect.height, 0, t / _showDuration);
				_element.anchoredPosition = position;
				_group.alpha = Mathf.Clamp01(t / _showDuration * 2);
			}
		}
		private async Task HideAnimate() {
			var t = 0f;
			while (t < _hideDuration) {
				t += Time.deltaTime;
				await Awaitable.NextFrameAsync();

				var position = _element.anchoredPosition;
				position.y = Mathf.Lerp(0, Screen.height / 2f + _element.rect.height, t / _hideDuration);
				_element.anchoredPosition = position;
				_group.alpha = Mathf.Clamp01(1 - t / _hideDuration * 2);
			}
		}
	}
}