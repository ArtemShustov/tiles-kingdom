using UnityEngine;
using UnityEngine.Events;

namespace Core.LiteLocalization {
	public class LocalizeStringEvent: MonoBehaviour {
		[SerializeField] private LocalizedString _localizedString;
		[SerializeField] private UnityEvent<string> _event;

		public void Refresh() {
			_event.Invoke(_localizedString.GetLocalized());
		}

		private void OnLangChanged() {
			Refresh();
		}
		private void OnEnable() {
			Localization.LanguageChanged += OnLangChanged;
			Refresh();
		}
		private void OnDisable() {
			Localization.LanguageChanged -= OnLangChanged;
		}
	}
}