using System.Linq;
using Core.Events;
using Game.Popups;
using UnityEngine;

namespace Game.Tiles.Buildings {
	public class Mine: Building {
		[SerializeField] private int _strategyPoints = 1;
		[SerializeField] private int _logisticsPoints = 1;
		[SerializeField] private int _time = 20;
		[SerializeField] private int _logisticsTime = 5;

		private int _counter;
		private int _timer;

		public override bool CanBuildAt(PlayGrid grid, Vector2Int position) {
			return grid.GetEightNeighbours(position)
				.All(c => c.Building.Value is not Mine && c.Building.Value is not Castle);
		}

		private void AddLogistics(int points) {
			Cell.Owner.Value.LogisticsPoints.Add(points);
			if (Cell.Owner.Value?.Flags.HasFlag(PlayerFlags.Human) == true) {
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(transform.position, new Color(0.8f, 0.7f, 0), $"+{points}"));
			}
		}
		private void AddStrategy(int points) {
			Cell.Owner.Value.StrategyPoints.Add(points);
			if (Cell.Owner.Value?.Flags.HasFlag(PlayerFlags.Human) == true) {
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(transform.position, Color.gray, $"+{points}"));
			}
		}
		
		private void OnTick(int ticks) {
			if (Cell?.Owner.Value == null) {
				return;
			}
			_timer += ticks;
			while (_timer >= _time) {
				_counter += 1;
				if (_counter >= _logisticsTime) {
					AddLogistics(_logisticsPoints);
					_counter = 0;
				} else {
					AddStrategy(_strategyPoints);
				}
				_timer -= _time;
			}
		}
		private void OnEnable() {
			LevelRoot.Tick += OnTick;
		}
		private void OnDisable() {
			LevelRoot.Tick -= OnTick;
		}
	}
}