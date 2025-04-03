using UnityEngine;
using YG;

namespace Game.YandexGames {
	public class FlagConfigurator: MonoBehaviour {
		[Header("Ad by timer")]
		[SerializeField] private string _advDelayKey = "InterstitialAdvDelay";
		[SerializeField] private int _defaultDelay = 60;
		[SerializeField] private string _timerAdEnabledKey = "TimerAdvEnabled";
		[SerializeField] private bool _defaultTimerAdEnabled = false;
		[SerializeField] private TimerBeforeAdsYG _timerBeforeAdsYG;
		[Header("Ad on win")]
		[SerializeField] private bool _defaultWinAdEnabled = true;
		[SerializeField] private AdOnWin _adOnWin;
		
		private void Awake() {
			// Ad by timer
			YG2.infoYG.InterstitialAdv.interAdvInterval = YG2.TryGetFlagAsInt(_advDelayKey, out int interstitialDelay) 
				? interstitialDelay : _defaultDelay;
			Debug.Log($"[CONFIG] YG2.infoYG.InterstitialAdv.interAdvInterval = {YG2.infoYG.InterstitialAdv.interAdvInterval}");
			
			_timerBeforeAdsYG.enabled = YG2.TryGetFlagAsBool(_timerAdEnabledKey, out bool timerAdEnabled) 
				? timerAdEnabled : _defaultTimerAdEnabled;
			Debug.Log($"[CONFIG] TimerAdv = {timerAdEnabled}");
			
			_adOnWin.enabled = _defaultWinAdEnabled;
			Debug.Log($"[CONFIG] AdOnWin - {_defaultWinAdEnabled}");
		}
	}
}