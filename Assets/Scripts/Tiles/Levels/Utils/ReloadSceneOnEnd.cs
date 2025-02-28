using Core.Events;
using Game.Tiles.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Tiles.Levels.Utils {
	public class ReloadSceneOnEnd: MonoBehaviour {
		[SerializeField] private float _delay = 1;

		public void Reload() {
			Invoke(nameof(ReloadScene), _delay);
		}
		
		private void ReloadScene() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnWin(PlayerWinEvent gameEvent) {
			Reload();
		}
		private void OnLose(PlayerLoseEvent gameEvent) {
			Reload();
		}
		private void OnEnable() {
			EventBus<PlayerLoseEvent>.Event += OnLose;
			EventBus<PlayerWinEvent>.Event += OnWin;
		}
		private void OnDisable() {
			EventBus<PlayerLoseEvent>.Event -= OnLose;
			EventBus<PlayerWinEvent>.Event -= OnWin;
		}
	}
}