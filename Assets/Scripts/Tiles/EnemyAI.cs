using System.Linq;
using Core;
using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles {
	public class EnemyAI : MonoBehaviour {
		[SerializeField] private float _time = 1f;
		[Range(0, 100)]
		[SerializeField] private int _fenceChance = 5;
		[Range(0, 100)]
		[SerializeField] private int _mineChance = 50;
		[SerializeField] private int _skipChance = 0;
		private Player _player;
		private LevelRoot _level;
		private Castle _castle;
		private GridPathFinder _finder;

		private float _timer;
		private float _inactivityTimer;

		private void Awake() {
			_time = PlayerProfile.Current.Difficulty switch {
				PlayerProfile.DifficultyLevel.Easy => 1.5f,
				PlayerProfile.DifficultyLevel.Normal => 1f,
				PlayerProfile.DifficultyLevel.Hard => 0.5f,
				_ => _time
			};
		}

		public void Init(Player player, LevelRoot level, Castle castle) {
			_player = player;
			_level = level;
			_castle = castle;
			_finder = new GridPathFinder(_level.Grid);
			_castle.Captured += OnCaptured;
		}
		public void SetTurnSkipChance(int change = 50) {
			_skipChance = change;
		}

		private void Update() {
			_timer += Time.deltaTime;

			if (_player.HasFlag(PlayerFlags.Cheating)) {
				_inactivityTimer += Time.unscaledDeltaTime;
				if (_inactivityTimer >= 5f) {
					_player.StrategyPoints.Add(Random.Range(10, 20));
					_player.LogisticsPoints.Add(Random.Range(3, 6));
					_inactivityTimer = 0f;
					// Debug.Log("DON'T BE LAZY");
				}
			}

			if (_timer < _time) return;

			_timer = 0f;
			AiUpdate();
		}

		private void AiUpdate() {
			if (_finder == null) return;
			if (Utils.Chance(_skipChance)) {
				return;
			}

			var cells = _finder.GetCellsNearOwned(_castle.Cell, _player);
			if (cells == null || cells.Length == 0) {
				return;
			}

			if (_player.HasFlag(PlayerFlags.SmartAI) && TryBuild(cells)) {
				return;
			}

			var target = cells.FirstOrDefault(c => c.Building.Value is Mine) ?? cells.GetRandom();
			var cost = target.GetCaptureCostFor(_player);
			if (_player.StrategyPoints.Take(cost)) {
				target.Capture(_player);
			} else if (_player.HasFlag(PlayerFlags.Cheating)) {
				if (target.Building.Value is Castle) {
					target.Capture(_player);
					Debug.Log($"HESOYAM by <color={_player.Color.ToHex()}>{_player.Color.ToHex()}</color>");
				}
			}
		}
		private bool TryBuild(Cell[] cells) {
			var randomCell = cells.GetRandom();
			
			var fenceCandidate = _level.Grid.GetNeighbours(randomCell.Position)
				.FirstOrDefault(c => c.Owner.Value == _player && c.Building.Value == null);
			if (CheckFence()) {
				_level.AttachFence(fenceCandidate.Position);
				return true;
			}

			var mineCandidate = _level.Grid.GetConnected(_castle.Cell.Position, c => c.Owner.Value == _player && c.Building.Value == null)
				.GetRandom();
			if (CheckMine()) {
				_level.AttachMine(mineCandidate.Position);
				return true;
			}

			return false;

			bool CheckMine() {
				return mineCandidate != null
				       && Utils.Chance(_mineChance)
				       && CheckMineRestriction(mineCandidate.Position)
				       && _player.LogisticsPoints.Take(5);
			}
			bool CheckFence() {
				return fenceCandidate != null && Utils.Chance(_fenceChance) && _player.LogisticsPoints.Take(1);
			}
			bool CheckMineRestriction(Vector2Int position) {
				return _level.Grid.GetEightNeighbours(position)
					.All(c => c.Building.Value is not Mine && c.Building.Value is not Buildings.Castle);
			}
		}

		private void OnCaptured(Player by) {
			enabled = false;
		}
		private void OnPlayerActed(PlayerActedEvent gameEvent) {
			_inactivityTimer = 0f;
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
