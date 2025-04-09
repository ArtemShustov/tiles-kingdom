using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles.Levels.Utils {
	public class MenuCanvas: MonoBehaviour {
		[SerializeField] private float _animationDuration = 0.5f;
		[Header("Components")]
		[SerializeField] private Button _soloMode;
		[SerializeField] private Button _duelMode;
		[SerializeField] private Button _onlineMode;
		private Level _soloLevel;
		private Level _duelLevel;
		private Level _onlineLevel;

		public Level SoloLevel {
			get => _soloLevel;
			set {
				_soloLevel = value;
				_soloMode.gameObject.SetActive(value != null);
			}
		}
		public Level DuelLevel {
			get => _duelLevel;
			set {
				_duelLevel = value;
				_duelMode.gameObject.SetActive(value != null);
			}
		}
		public Level OnlineLevel {
			get => _onlineLevel;
			set {
				_onlineLevel = value;
				_onlineMode.gameObject.SetActive(value != null);
			}
		}

		private void OnSoloClicked() {
			LevelBoot.LevelToLoad = SoloLevel;
			LevelBoot.Restart();
		}
		private void OnDuelClicked() {
			LevelBoot.LevelToLoad = DuelLevel;
			LevelBoot.Restart();
		}
		private void OnOnlineClicked() {
			LevelBoot.LevelToLoad = OnlineLevel;
			LevelBoot.Restart();
		}
		private async Task Animate() {
			var t = 0f;
			while (t < _animationDuration) {
				t += Time.deltaTime;
				var scale = Vector3.one * Mathf.Lerp(0f, 1f, t / _animationDuration);
				_soloMode.transform.localScale = scale;
				_duelMode.transform.localScale = scale;
				_onlineMode.transform.localScale = scale;
				await Awaitable.NextFrameAsync();
			}
			var finalScale = Vector3.one;
			_soloMode.transform.localScale = finalScale;
			_duelMode.transform.localScale = finalScale;
			_onlineMode.transform.localScale = finalScale;
		}

		private void OnEnable() {
			_soloMode.onClick.AddListener(OnSoloClicked);
			_soloMode.gameObject.SetActive(_soloLevel != null);
			_duelMode.onClick.AddListener(OnDuelClicked);
			_duelMode.gameObject.SetActive(_duelLevel != null);
			_onlineMode.onClick.AddListener(OnOnlineClicked);
			_onlineMode.gameObject.SetActive(_onlineLevel != null);
			Animate().Forget();
		}
		private void OnDisable() {
			_soloMode.onClick.RemoveListener(OnSoloClicked);
			_duelMode.onClick.RemoveListener(OnDuelClicked);
			_onlineMode.onClick.RemoveListener(OnOnlineClicked);
		}
	}
}