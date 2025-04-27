using System.Threading.Tasks;
using Core.Events;
using Game.Tiles.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Tiles.Levels.Utils {
	public class ReloadSceneOnEnd: MonoBehaviour {
		[SerializeField] private float _delay = 1f;
		private LevelRoot _root;

		public void SetRoot(LevelRoot root) {
			_root = root;
		}
		private void ReloadScene() {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void OnWin(PlayerWinEvent gameEvent) {
			Time.timeScale = 1;
			WinAsync().Forget();
		}
		private async Task WinAsync() {
			if (_root) {
				await _root.UI.WinPanel.ShowAndHideAsync();
			} else {
				await Awaitable.WaitForSecondsAsync(_delay);
			}
			ReloadScene();
		}
		
		private void OnLose(PlayerLoseEvent gameEvent) {
			Time.timeScale = 1;
			LoseAsync().Forget();
		}
		private async Task LoseAsync() {
			if (_root) {
				await _root.UI.LosePanel.ShowAndHideAsync();
			} else {
				await Awaitable.WaitForSecondsAsync(_delay);
			}
			ReloadScene();
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