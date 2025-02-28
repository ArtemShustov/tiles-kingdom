using System.Collections.Generic;
using UnityEngine;

namespace Core {
	public static class Extensions {
		public static string ToHex(this Color color, bool includeAlpha = true) {
			return includeAlpha 
				? $"#{ColorUtility.ToHtmlStringRGBA(color)}"
				: $"#{ColorUtility.ToHtmlStringRGB(color)}";
		}
		public static T GetRandom<T>(this IList<T> list) {
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		public static T GetRandom<T>(this T[] list) {
			return list[UnityEngine.Random.Range(0, list.Length)];
		}
	}
}