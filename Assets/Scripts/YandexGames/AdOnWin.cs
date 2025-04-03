using Core.Events;
using Game.Tiles.Events;
using UnityEngine;
using YG;
using YG.Insides;

namespace Game.YandexGames {
	public class AdOnWin: MonoBehaviour {
		private void OnPlayerWin(PlayerWinEvent gameevent) {
			Debug.Log("PLAYER WIN");
			YGInsides.ResetTimerInterAdv(); // ???
			YG2.InterstitialAdvShow();
		}
		private void OnEnable() {
			EventBus<PlayerWinEvent>.Event += OnPlayerWin;
		}
		private void OnDisable() {
			EventBus<PlayerWinEvent>.Event -= OnPlayerWin;
		}
	}
}