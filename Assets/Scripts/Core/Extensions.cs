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
	}
}