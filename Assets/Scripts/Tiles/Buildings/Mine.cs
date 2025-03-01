using Core.Events;
using Game.Tiles.Popups;
using UnityEngine;

namespace Game.Tiles.Buildings {
	public class Mine: Building {
		[SerializeField] private int _logisticsCount = 5;
		[SerializeField] private int _points = 1;
		[SerializeField] private float _time = 2f;

		private int _counter;
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
			if (_counter >= _logisticsCount) {
				AddLogistics();
				_counter = 0;
			} else {
				AddStrategy();
				_counter += 1;
			}
		}
		private void AddLogistics() {
			Cell.Owner.Value.LogisticsPoints.Add(_points);
			if (Cell.Owner.Value?.Flags.HasFlag(PlayerFlags.Human) == true) {
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(transform.position, new Color(0.8f, 0.7f, 0), $"+{_points}"));
			}
		}
		private void AddStrategy() {
			Cell.Owner.Value.StrategyPoints.Add(_points);
			if (Cell.Owner.Value?.Flags.HasFlag(PlayerFlags.Human) == true) {
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(transform.position, Color.gray, $"+{_points}"));
			}
		}
	}
}