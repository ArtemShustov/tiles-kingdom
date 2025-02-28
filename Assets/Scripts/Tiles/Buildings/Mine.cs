using Core.Events;
using Game.Tiles.Popups;
using UnityEngine;

namespace Game.Tiles.Buildings {
	public class Mine: Building {
		[SerializeField] private int _points = 1;
		[SerializeField] private float _time = 2f;

		private float _timer;

		private void Update() {
			if (Cell?.Owner.Value == null) {
				return;
			}
			_timer += Time.deltaTime;
			if (_timer >= _time) {
				_timer = 0;
				OnTimerEnded();
			}
		}
		private void OnTimerEnded() {
			Cell.Owner.Value.StrategyPoints.Add(_points);
			if (Cell.Owner.Value?.Flags.HasFlag(PlayerFlags.Human) == true) {
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(transform.position, Color.gray, $"+{_points}"));
			}
		}
	}
}