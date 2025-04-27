using UnityEngine;

namespace Core {
	public static class Extensions {
		public static string ToHex(this Color color, bool includeAlpha = true) {
			return includeAlpha 
				? $"#{ColorUtility.ToHtmlStringRGBA(color)}"
				: $"#{ColorUtility.ToHtmlStringRGB(color)}";
		}
		public static T GetRandom<T>(this T[] list) {
			if (list == null || list.Length == 0) {
				return default(T);
			}
			return list[UnityEngine.Random.Range(0, list.Length)];
		}
		public static float RandomBetween(this Vector2 vector2) {
			return UnityEngine.Random.Range(vector2.x, vector2.y);
		}
		public static int RandomBetween(this Vector2Int vector2) {
			return UnityEngine.Random.Range(vector2.x, vector2.y);
		}
	}
}