using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles.UI {
	public class PlayerUI: MonoBehaviour {
		[Header("Main")]
		[SerializeField] private WalletView _strategyView;
		[SerializeField] private WalletView _logisticsView;
		[SerializeField] private GameObject _container;
		[Header("Background")]
		[SerializeField] private RectTransform _background;
		[SerializeField] private Image _backgroundImage;
		[SerializeField] private float _backgroundAnimDuration = 0.2f;
		[SerializeField] private Vector2 _hideSize = new Vector2(0, -85);
		[SerializeField] private Vector2 _hidePosition = new Vector2(0, 85 / 2f);

		public bool Background {
			get => _background.gameObject.activeSelf;
			set => _background.gameObject.SetActive(value);
		}
		
		private Player _player;

		private void Awake() {
			Background = false;
			HideTurnLabelNow();
		}

		public async Task HideTurnLabelAsync() {
			var t = 0f;
			var finalSize = _hideSize; 
			var finalPosition = _hidePosition;
			while (t < _backgroundAnimDuration) {
				_background.sizeDelta = Vector2.Lerp(Vector2.zero, finalSize, t / _backgroundAnimDuration);
				_background.anchoredPosition = Vector2.Lerp(Vector2.zero, finalPosition, t / _backgroundAnimDuration);
				t += Time.deltaTime;
				await Awaitable.NextFrameAsync();
			}
			_background.sizeDelta = finalSize;
			_background.anchoredPosition = finalPosition;
		}
		public void HideTurnLabelNow() {
			_background.sizeDelta = _hideSize;
			_background.anchoredPosition = _hidePosition;
		}
		public async Task ShowTurnLabelAsync() {
			var t = 0f;
			var startSize = _hideSize; 
			var startPosition = _hidePosition;
			while (t < _backgroundAnimDuration) {
				_background.sizeDelta = Vector2.Lerp(startSize, Vector2.zero, t / _backgroundAnimDuration);
				_background.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, t / _backgroundAnimDuration);
				t += Time.deltaTime;
				await Awaitable.NextFrameAsync();
			}
			_background.sizeDelta = Vector2.zero;
			_background.anchoredPosition = Vector2.zero;
		}
		public void ShowTurnLabelNow() {
			_background.sizeDelta = Vector2.zero;
			_background.anchoredPosition = Vector2.zero;
		}

		public void Bind(Player player) {
			_player = player;
			_strategyView.Bind(_player.StrategyPoints);
			_logisticsView.Bind(_player.LogisticsPoints);
			_container.SetActive(_player != null);
			_backgroundImage.color = player.Color;
		}
		private void OnEnable() {
			_container.SetActive(_player != null);
		}
	}
}