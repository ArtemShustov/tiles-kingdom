using Core.LiteLocalization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Game {
	public static class LocaleSelector {
		private const string EN = "en";
		private const string RU = "ru";
		private const string PREFS_KEY = "locale";
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize() {
			var locale = EN;
			if (TryEditor(out locale) || TrySDK(out locale)) {
				locale ??= EN;
				Localization.ChangeLanguage(locale);
			}
		}

		private static bool TryEditor(out string locale) {
			#if UNITY_EDITOR
			locale = EditorPrefs.GetString(PREFS_KEY, string.Empty);
			return !string.IsNullOrEmpty(locale);
			#else
			locale = string.Empty;
			return false;
			#endif
		}
		private static bool TrySDK(out string locale) { // YG
			locale = string.Empty;
			return false;
		}
	}
}