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
			if (task == null) {
				return;
			}

			task.ContinueWith(t => {
				if (t.Exception != null) {
					Debug.LogError($"Error in Task: {t.Exception.InnerException}");
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
		public static bool Chance(int chance) {
			chance = Mathf.Clamp(chance, 0, 100);
			return UnityEngine.Random.Range(0, 100) <= chance;
		}
		public static Color GetRandomNiceColor() {
			float h = Random.value;
			while (Mathf.Abs(h - 0.6f) < 0.1f) {
				h = Random.value;
			} 
			float s = Random.Range(0.5f, 1f);
			float v = Random.Range(0.5f, 1f);

			return Color.HSVToRGB(h, s, v);
		}
	}
}