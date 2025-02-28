using Core.Events;
using UnityEngine;

namespace Game.Tiles.Popups {
	public class PopupSpawner: MonoBehaviour {
		[SerializeField] private WorldPopup _prefab;
		
		private void OnShowEvent(ShowPopupEvent gameEvent) {
			var popup = Instantiate(_prefab, transform);
			popup.transform.position = gameEvent.Position;
			popup.Setup(gameEvent.Text, gameEvent.Color);
		}
		private void OnEnable() {
			EventBus<ShowPopupEvent>.Event += OnShowEvent;
		}
		private void OnDisable() {
			EventBus<ShowPopupEvent>.Event -= OnShowEvent;
		}
	}
}