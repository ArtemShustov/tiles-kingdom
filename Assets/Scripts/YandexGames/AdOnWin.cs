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
		private void OnPlayerLose(PlayerLoseEvent gameEvent) {
			Debug.Log("PLAYER LOSE");
			YGInsides.ResetTimerInterAdv(); // ???
			YG2.InterstitialAdvShow();
		}
		private void OnEnable() {
			EventBus<PlayerWinEvent>.Event += OnPlayerWin;
			EventBus<PlayerLoseEvent>.Event += OnPlayerLose;
		}
		private void OnDisable() {
			EventBus<PlayerWinEvent>.Event -= OnPlayerWin;
			EventBus<PlayerLoseEvent>.Event -= OnPlayerLose;
		}
	}
}