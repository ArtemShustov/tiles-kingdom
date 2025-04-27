using Core.LiteLocalization;
using UnityEditor;
using UnityEngine;

namespace Game.Editor {
	[InitializeOnLoad]
	public static class LocaleSelectorEditor {
		private const string PREFS_KEY = "locale";
		private const string EN_PATH = "Tools/Locale/English";
		private const string RU_PATH = "Tools/Locale/Russian";
		
		static LocaleSelectorEditor() {
			RefreshChecked();
		}

		[MenuItem(EN_PATH)]
		private static void SetEnglish() => SetLocale("en");
		[MenuItem(RU_PATH)]
		private static void SetRussian() => SetLocale("ru");

		private static void SetLocale(string locale) {
			EditorPrefs.SetString(PREFS_KEY, locale);
			RefreshChecked();
			if (Application.isPlaying) {
				Localization.ChangeLanguage(locale);
			}
		}
		private static void RefreshChecked() {
			var locale = EditorPrefs.GetString(PREFS_KEY, string.Empty);
			Menu.SetChecked(EN_PATH, locale == "en");
			Menu.SetChecked(RU_PATH, locale == "ru");
		}
	}
}