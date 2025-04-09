using Game.Tiles.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles.UI {
	public class LevelButton: MonoBehaviour {
		[SerializeField] private Button _button;
		[SerializeField] private Level _level;

		public Level Level {
			get => _level;
			set {
				_level = value;
				_button.gameObject.SetActive(_level != null);
			}
		}

		private void OnClick() {
			LevelBoot.LevelToLoad = _level;
			LevelBoot.Restart();
		}
		private void OnEnable() {
			_button.onClick.AddListener(OnClick);
			_button.gameObject.SetActive(_level != null);
		}
		private void OnDisable() {
			_button.onClick.RemoveListener(OnClick);
		}
	}
}