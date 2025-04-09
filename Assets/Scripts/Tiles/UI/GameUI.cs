using Game.Tiles.Levels;
using UnityEngine;

namespace Game.Tiles.UI {
	public class GameUI: MonoBehaviour {
		[SerializeField] private GameObject _timescaleButton;
		[SerializeField] private LevelButton _menuButton;
		[SerializeField] private GameObject _skipTurnButton;
		[field: SerializeField] public PlayerUI MainPlayer { get; private set; }
		[field: SerializeField] public PlayerUI SecondPlayer { get; private set; }

		public Level MenuLevel {
			get => _menuButton.Level;
			set => _menuButton.Level = value;
		}

		private void Awake() {
			_skipTurnButton.SetActive(false);
		}

		public void SetTimescaleButton(bool isEnabled) {
			_timescaleButton.SetActive(isEnabled);
		}
		public void SetSkipTurnButton(bool isEnabled) {
			_skipTurnButton.SetActive(isEnabled);
		}
	}
}