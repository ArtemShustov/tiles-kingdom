using System;
using Game.Tiles.Buildings;
using UnityEngine;

namespace Game.Tiles {
	public class EnemyAI: MonoBehaviour {
		[SerializeField] private float _time = 1;
		private Player _player;
		private PlayGrid _grid;
		private Castle _castle;

		private float _timer;

		public Player Player => _player;

		private void Awake() {
			_time = PlayerProfile.Current.Difficulty switch {
				PlayerProfile.DifficultyLevel.Easy => 1.5f,
				PlayerProfile.DifficultyLevel.Normal => 1,
				PlayerProfile.DifficultyLevel.Hard => 0.5f,
				_ => _time
			};
		}
		public void Init(Player player, PlayGrid grid, Castle castle) {
			_player = player;
			_grid = grid;
			_castle = castle;
			_castle.Captured += OnCaptured;
		}
		private void OnCaptured(Player obj) {
			enabled = false;
		}

		private void Update() {
			_timer += Time.deltaTime;
			if (_timer >= _time) {
				_timer = 0f;
				AiUpdate();
			}
		}
		private void AiUpdate() {
			var finder = new GridPathFinder(_grid);
			var cell = finder.GetCellNearOwned(_castle.Cell, _player);
			if (cell == null) {
				return;
			}

			var cost = cell.GetCaptureCostFor(_player);
			if (_player.StrategyPoints.Take(cost)) {
				cell.Capture(_player);
			}
		}

		private void OnDestroy() {
			if (_castle) {
				_castle.Captured -= OnCaptured;
			}
		}
	}
}