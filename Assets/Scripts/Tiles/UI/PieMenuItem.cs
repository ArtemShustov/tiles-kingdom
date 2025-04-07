using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tiles.UI {
	public class PieMenuItem: MonoBehaviour {
		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _costLabel;
		[field: SerializeField] public Building Prefab { get; private set; }
		[field: SerializeField] public int Cost { get; private set; }
		
		public event Action<PieMenuItem> Selected;

		public void SetLabelColor(Color color) {
			_costLabel.color = color;
		}
		
		private void OnClick() {
			Selected?.Invoke(this);
		}
		private void OnEnable() {
			_button.onClick.AddListener(OnClick);
			_costLabel.text = Cost.ToString();
		}
		private void OnDisable() {
			_button.onClick.RemoveListener(OnClick);
		}
	}
}