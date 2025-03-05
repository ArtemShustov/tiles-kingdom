#if PLUGIN_YG_2
using UnityEngine;
using YG;
#endif
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game.YandexGames {
	public class YandexLocaleSelector: IStartupLocaleSelector {
		public Locale GetStartupLocale(ILocalesProvider availableLocales) {
			#if PLUGIN_YG_2
			var locale = availableLocales.GetLocale(YG2.envir.language);
			if (locale) {
				Debug.Log($"Selected '{locale.LocaleName}' locale by {nameof(YandexLocaleSelector)}");
			}
			return locale;
			#else
			return null
			#endif
		}
	}
}