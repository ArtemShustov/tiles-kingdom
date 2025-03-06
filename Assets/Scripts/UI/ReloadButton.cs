using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI {
	public class ReloadButton: MonoBehaviour {
		[SerializeField] private Button _button;
		
		private void OnClick() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		private void OnEnable() {
			_button.onClick.AddListener(OnClick);
		}
		private void OnDisable() {
			_button.onClick.RemoveListener(OnClick);
		}
		private void OnDestroy() {
			Time.timeScale = 1.0f;
		}
	}
}