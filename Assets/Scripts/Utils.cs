using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game {
	public static class Utils {
		public static bool IsPaused() {
			return Mathf.Approximately(Time.timeScale, 0f);
		}
		public static bool IsPointerOverUIObject() {
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}
		public static void Forget(this Task task) {
			if (task == null || task.IsCompleted) {
				return;
			}
			task.ContinueWith(t => {
				if (t.Exception != null) {
					Debug.LogError($"Error in Task: {t.Exception.InnerException}");
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}