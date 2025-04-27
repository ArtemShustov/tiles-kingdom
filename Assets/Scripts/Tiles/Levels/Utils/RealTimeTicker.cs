using UnityEngine;

namespace Game.Tiles.Levels.Utils {
	public class RealTimeTicker: MonoBehaviour {
		[SerializeField] private int _ticksPerSecond = 10;
		private float _timer;

		private void Update() {
			_timer += Time.deltaTime;
			var tickDelta = 1f / _ticksPerSecond;
			var ticks = Mathf.FloorToInt(_timer / tickDelta);
			if (ticks > 0) {
				_timer -= tickDelta * ticks;
				LevelRoot.TickAll(ticks);
			}
		}
	}
}