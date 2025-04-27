using System;
using Core.Events;
using Game.Tiles.Events;
using UnityEngine;
using YG;
using YG.Insides;

namespace Game.YandexGames {
	public class AdOnWin: MonoBehaviour {
		private void Awake() {
			YG2.infoYG.InterstitialAdv.interAdvInterval = YG2.TryGetFlagAsInt("InterstitialAdvDelay", out int interstitialDelay) 
				? interstitialDelay : YG2.infoYG.InterstitialAdv.interAdvInterval;
			Debug.Log($"[CONFIG] Adv: {YG2.infoYG.InterstitialAdv.interAdvInterval}");
		}
		private void OnPlayerWin(PlayerWinEvent gameevent) {
			Debug.Log($"PLAYER WIN. Time to ad: {YG2.timerInterAdv}");
			// YGInsides.ResetTimerInterAdv();
			if (YG2.isTimerAdvCompleted || Mathf.Approximately(YGInsides.timeShowInterAdv, YG2.infoYG.InterstitialAdv.interAdvInterval)) {
				YG2.InterstitialAdvShow();
			}
		}
		private void OnPlayerLose(PlayerLoseEvent gameEvent) {
			Debug.Log($"PLAYER LOSE. Time to ad: {YG2.timerInterAdv}");
			// YGInsides.ResetTimerInterAdv();
			if (YG2.isTimerAdvCompleted || Mathf.Approximately(YGInsides.timeShowInterAdv, YG2.infoYG.InterstitialAdv.interAdvInterval)) {
				YG2.InterstitialAdvShow();
			}
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