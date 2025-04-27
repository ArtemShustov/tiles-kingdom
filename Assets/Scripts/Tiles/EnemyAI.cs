using System.Linq;
using Core;
using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Tiles {
	public class EnemyAI : MonoBehaviour {
		[SerializeField] private DifficultyRandomTable _turnTimeTable;
		[SerializeField] private DifficultyRandomTable _cheatTimeTable;
		[Range(0, 100)]
		[SerializeField] private int _fenceChance = 5;
		[Range(0, 100)]
		[SerializeField] private int _mineChance = 50;

		private int _skipTurnChance;
		private float _turnTime;
		private float _cheatTime;
		
		private Player _player;
		private LevelRoot _level;
		private Castle _castle;
		private GridPathFinder _finder;

		private float _turnTimer;
		private float _cheatTimer;

		private void Awake() {
			_turnTime = _turnTimeTable.Get();
			_cheatTime = _cheatTimeTable.Get();
		}

		public void Init(Player player, LevelRoot level, Castle castle) {
			_player = player;
			_level = level;
			_castle = castle;
			_finder = new GridPathFinder(_level.Grid);
			_castle.Captured += OnCaptured;
		}
		public void SetTurnSkipChance(int change = 50) {
			_skipTurnChance = change;
		}

		private void Update() {
			if (_player == null || !_level || !_castle) {
				return;
			}
			// Cheating
			if (_player.HasFlag(PlayerFlags.Cheating) && _cheatTime != 0) {
				_cheatTimer += Time.unscaledDeltaTime;
				if (_cheatTimer >= _cheatTime) {
					_player.StrategyPoints.Add(Random.Range(2, 10));
					_player.LogisticsPoints.Add(Random.Range(1, 5));
					_cheatTimer = 0f;
				}
			}

			// Turn
			_turnTimer += Time.deltaTime;
			if (_turnTimer >= _turnTime) {
				_turnTimer = 0f;
				AiUpdate();
			}
		}

		private void AiUpdate() {
			if (_finder == null) return;
			if (Utils.Chance(_skipTurnChance)) {
				return;
			}

			var cells = _finder.GetCellsNearOwned(_castle.Cell, _player);
			if (cells == null || cells.Length == 0) {
				return;
			}

			// Building
			if (_player.HasFlag(PlayerFlags.CanBuild)) {
				TryBuild(cells);
			}

			// Capture
			var captureTarget = cells.FirstOrDefault(c => c.Building.Value is Mine) ?? cells.GetRandom();
			var cost = captureTarget.GetCaptureCostFor(_player);
			if (_player.StrategyPoints.Take(cost)) {
				captureTarget.Capture(_player);
			} else if (_player.HasFlag(PlayerFlags.Cheating)) {
				if (captureTarget.Building.Value is Castle) {
					captureTarget.Capture(_player);
				}
			}
		}
		private bool TryBuild(Cell[] cellsNearOwned) {
			// fence
			var fenceCandidate = _level.Grid
				.GetNeighbours(cellsNearOwned.GetRandom().Position)
				.FirstOrDefault(c => c.Owner.Value == _player && c.Building.Value == null);
			if (CheckFence()) {
				_level.AttachFence(fenceCandidate.Position);
				return true;
			}

			// mine
			var mineCandidate = _level.Grid
				.GetConnected(_castle.Cell.Position, c => c.Owner.Value == _player && c.Building.Value == null)
				.GetRandom();
			if (CheckMine()) {
				_level.AttachMine(mineCandidate.Position);
				return true;
			}

			return false;

			bool CheckMine() {
				return mineCandidate != null
				       && Utils.Chance(_mineChance)
				       && _level.MinePrefab.CanBuildAt(_level.Grid, mineCandidate.Position)
				       && _player.LogisticsPoints.Take(5);
			}
			bool CheckFence() {
				return fenceCandidate != null 
				       && Utils.Chance(_fenceChance) 
				       && _player.LogisticsPoints.Take(1);
			}
		}

		private void OnCaptured(Player by) {
			enabled = false;
		}
		private void OnPlayerActed(PlayerActedEvent gameEvent) {
			_cheatTimer = 0f;
		}
		private void OnDestroy() {
			if (_castle != null) {
				_castle.Captured -= OnCaptured;
			}
		}
		private void OnEnable() {
			EventBus<PlayerActedEvent>.Event += OnPlayerActed;
		}
		private void OnDisable() {
			EventBus<PlayerActedEvent>.Event -= OnPlayerActed;
		}
	}
}
