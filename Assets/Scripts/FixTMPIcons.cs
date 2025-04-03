using TMPro;
using UnityEngine;

namespace Game {
	public class FixTMPIcons: MonoBehaviour {
		[SerializeField] private TMP_Text _label;

		private void Awake() {
			if (_label.spriteAsset == null) {
				_label.spriteAsset = TMP_Settings.defaultSpriteAsset;
			}
		}
	}
}