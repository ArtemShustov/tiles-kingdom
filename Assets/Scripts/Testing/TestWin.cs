using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Testing {
	public class TestWin: MonoBehaviour {
		[SerializeField] private float _time = 2;
		private Castle _castle;
		
		private void RestartScene() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnWin(PlayerWinEvent gameEvent) {
			Invoke(nameof(RestartScene), _time);
		}
		private void OnLose(PlayerLoseEvent gameEvent) {
			Invoke(nameof(RestartScene), _time);
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