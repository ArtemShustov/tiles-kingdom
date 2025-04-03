using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.LiteLocalization {
	[Serializable]
	public class LocalizedString {
		[field: SerializeField] public string Key { get; set; }
		[SerializeField] private LocalizedPair[] _pairs = {
			new LocalizedPair() { Lang = "en" },
			new LocalizedPair() { Lang = "ru" }
		};
		
		public IReadOnlyCollection<LocalizedPair> Pairs => _pairs;

		public string GetLocalized(string lang) {
			return _pairs.FirstOrDefault(p => p.Lang == lang).Value ?? string.Empty;
		}
		public string GetLocalized() {
			return GetLocalized(Localization.Lang);
		}
		
		[Serializable]
		public struct LocalizedPair {
			public string Lang;
			[TextArea]
			public string Value;
		}
	}
}