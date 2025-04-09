using TMPro;
using UnityEngine;

namespace Game.Tiles.UI {
	public class WalletView: MonoBehaviour {
		[SerializeField] private string _pattern = "{0}";
		[SerializeField] private TMP_Text _label;

		private Wallet _wallet;

		public void Bind(Wallet wallet) {
			if (_wallet != null) {
				Unsubscribe(_wallet);
			}
			_wallet = wallet;
			Subscribe(_wallet);
			Refresh();
		}
		
		private void Refresh() {
			_label.text = string.Format(_pattern, _wallet.Value);
		}
		private void Subscribe(Wallet wallet) {
			wallet.ValueChanged += Refresh;
		}
		private void Unsubscribe(Wallet wallet) {
			wallet.ValueChanged -= Refresh;
		}

		private void OnEnable() {
			if (_wallet != null) {
				Subscribe(_wallet);
				Refresh();
			} else {
				_label.text = string.Format(_pattern, 0);
			}
		}
		private void OnDisable() {
			if (_wallet != null) {
				Unsubscribe(_wallet);
			}
		}
	}
}