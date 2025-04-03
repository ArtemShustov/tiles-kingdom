using System;
using UnityEngine;

namespace Core.LiteLocalization {
	public class Localization {
		private string _lang = "en";
		public static string Lang => _instance._lang;
		
		private event Action _languageChanged;
		public static event Action LanguageChanged {
			add => _instance._languageChanged += value;
			remove => _instance._languageChanged -= value;
		}

		public static void ChangeLanguage(string lang) {
			_instance._lang = lang;
			_instance._languageChanged?.Invoke();
		}
		
		private static Localization _instance;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void Initialize() {
			_instance = new Localization();
		}
	}
}