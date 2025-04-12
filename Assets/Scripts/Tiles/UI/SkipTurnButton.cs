using Core.Events;
using Game.Tiles.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles.UI {
	public class SkipTurnButton: MonoBehaviour {
		[SerializeField] private Button _button;

		private void OnClick() {
			EventBus<PlayerActedEvent>.Raise(new PlayerActedEvent(PlayerActedEvent.ActionType.None));
		}
		private void OnEnable() {
			_button.onClick.AddListener(OnClick);
		}
		private void OnDisable() {
			_button.onClick.RemoveListener(OnClick);
		}
	}
}