using System;
using Core;
using Game.Tiles.Buildings;
using Game.Tiles.PlayerSystems;
using Game.Tiles.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tiles {
	public class LevelRoot: MonoBehaviour {
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private PlayerUI _playerUI;
		[SerializeField] private LevelEnd _end;
		[SerializeField] private RequirePlayerMono[] _requirePlayer;
		[Header("Prefabs")]
		[SerializeField] private Cell _cellPrefab;
		[SerializeField] private Castle _castlePrefab;
		[SerializeField] private Mine _minePrefab;
		[SerializeField] private Tower _towerPrefab;
		[FormerlySerializedAs("_horsePrefab")]
		[SerializeField] private Fence _fencePrefab;
		
		private Player _player = new Player(Color.blue, PlayerFlags.Human);

		public Player Player => _player;
		public PlayGrid Grid => _grid;
		
		public void Awake() {
			_playerUI.Bind(_player);
		}
		#if DEBUG || UNITY_EDITOR || DEVELOPMENT_BUILD
		public void Update() {
			if (Input.GetKeyDown(KeyCode.R)) {
				_player.StrategyPoints.Add(1);
			}
		}
		#endif

		public Cell GetCell(Vector2Int position) => _grid.GetCell(position);
		public bool TryGetCell(Vector2Int position, out Cell cell) => _grid.TryGetCell(position, out cell);

		#region Players
		public void SetPlayerCastle(Castle castle) {
			_end.SetPlayer(castle, _player);
			foreach (var requirePlayerMono in _requirePlayer) {
				requirePlayerMono.Bind(castle, _player);
			}
		}
		
		public Player AddEnemy(Castle castle) => AddEnemy(UnityEngine.Random.ColorHSV(), castle);
		public Player AddEnemy(Color color, Castle castle) {
			var enemy = new Player(color, PlayerFlags.AI);
			_end.AddEnemy(castle, enemy);
			return enemy;
		}
		public Player AddEnemy(Player enemy, Castle castle) {
			_end.AddEnemy(castle, enemy);
			return enemy;
		}
		public EnemyAI AddAI(Player player, Castle castle) {
			var ai = new GameObject($"AI {player.Color.ToHex(false)}", typeof(EnemyAI)).GetComponent<EnemyAI>();
			ai.transform.parent = transform;
			ai.Init(player, _grid, castle);
			return ai;
		}
		#endregion

		#region Building
		public Castle AttachCastle(Vector2Int position) => AttachBuilding(position, _castlePrefab);
		public Mine AttachMine(Vector2Int position) => AttachBuilding(position, _minePrefab);
		public Tower AttachTower(Vector2Int position) => AttachBuilding(position, _towerPrefab);
		public Fence AttachFence(Vector2Int position) => AttachBuilding(position, _fencePrefab);
		public T AttachBuilding<T>(Vector2Int position, T buildingPrefab) where T: Building {
			var cell = _grid.GetCell(position);
			if (!cell) {
				cell = PlaceEmptyCell(position);
			}
			if (cell.Building.Value != null) {
				throw new InvalidOperationException($"Cell at position {position} is already has building");
			}
			var building = Instantiate(buildingPrefab, cell.transform);
			building.transform.localPosition = Vector3.zero + Vector3.back;
			cell.SetBuilding(building);
			return building;
		}
		#endregion
		
		public Cell PlaceEmptyCell(Vector2Int position) {
			if (_grid.HasCell(position)) {
				throw new InvalidOperationException($"Cell at position {position} is already exists");
			}
			var cell = Instantiate(_cellPrefab, _grid.transform);
			cell.transform.position = _grid.GetCellCenterWorld(position);
			cell.Init(_grid, position);
			_grid.SetCell(position, cell);
			return cell;
		}
	}
}