using System;
using Core;
using Game.Tiles.Buildings;
using Game.Tiles.PlayerSystems;
using Game.Tiles.UI;
using UnityEngine;

namespace Game.Tiles {
	public class LevelRoot: MonoBehaviour {
		[Header("Components")]
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private LevelEnd _end;
		[SerializeField] private PlayerUI _playerUI;
		[SerializeField] private RequirePlayerMono[] _requirePlayer;
		[SerializeField] private CameraMovement _cameraMovement;
		[SerializeField] private PlayerBuilderWithMenu _builder;
		[Header("Prefabs")]
		[SerializeField] private Cell _cellPrefab;
		[SerializeField] private Castle _castlePrefab;
		[SerializeField] private Mine _minePrefab;
		[SerializeField] private Tower _towerPrefab;
		[SerializeField] private Fence _fencePrefab;
		private Player _player;

		public Player Player => _player;
		public PlayGrid Grid => _grid;
		
		public Cell GetCell(Vector2Int position) => _grid.GetCell(position);
		public bool TryGetCell(Vector2Int position, out Cell cell) => _grid.TryGetCell(position, out cell);

		#region Players
		/// <summary>
		/// Sets up a human player
		/// </summary>
		/// <param name="player">Player</param>
		/// <param name="castle">Main castle of the player</param>
		public void SetPlayer(Player player, Castle castle) {
			_player = player;
			_end.SetPlayer(castle, _player);
			_playerUI.Bind(_player);
			foreach (var requirePlayerMono in _requirePlayer) {
				requirePlayerMono.Bind(castle, _player);
			}
		}
		/// <summary>
		/// Adds new enemy
		/// </summary>
		public Player AddEnemy(Castle castle) => AddEnemy(UnityEngine.Random.ColorHSV(), castle);
		/// <summary>
		/// Adds new enemy
		/// </summary>
		public Player AddEnemy(Color color, Castle castle) {
			var enemy = new Player(color, PlayerFlags.AI);
			_end.AddEnemy(castle, enemy);
			return enemy;
		}
		/// <summary>
		/// Adds new enemy
		/// </summary>
		public Player AddEnemy(Player enemy, Castle castle) {
			_end.AddEnemy(castle, enemy);
			return enemy;
		}
		/// <summary>
		/// Adds AI to an existing enemy
		/// </summary>
		/// <param name="player">Enemy player</param>
		/// <param name="castle">Existing castle</param>
		/// <returns>Created AI brain</returns>
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
		
		#region Common
		/// <summary>
		/// Sets the availability of free movement of the camera
		/// </summary>
		public void SetFreeCameraAllowed(bool allow) {
			if (allow) {
				_cameraMovement.EnableInput();
			} else {
				_cameraMovement.DisableInput();
			}
		}
		/// <summary>
		/// Sets the availability of building
		/// </summary>
		public void SetBuildingAllowed(bool allow) {
			_builder.enabled = allow;
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