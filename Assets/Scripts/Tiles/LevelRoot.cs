using System;
using Core;
using Game.Tiles.Buildings;
using Game.Tiles.PlayerSystems;
using Game.Tiles.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tiles {
	public class LevelRoot: MonoBehaviour {
		[Header("Components")]
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private GameUI _gameUI;
		[FormerlySerializedAs("_requirePlayer")]
		[SerializeField] private PlayerSystem[] _playerSystems;
		[SerializeField] private CameraMovement _cameraMovement;
		[SerializeField] private PlayerBuilderWithMenu _builder;
		[Header("Prefabs")]
		[SerializeField] private Cell _cellPrefab;
		[SerializeField] private Castle _castlePrefab;
		[SerializeField] private Mine _minePrefab;
		[SerializeField] private Tower _towerPrefab;
		[SerializeField] private Fence _fencePrefab;
		public PlayGrid Grid => _grid;
		public GameUI UI => _gameUI;
		
		public static event Action<int> Tick;
		
		public Cell GetCell(Vector2Int position) => _grid.GetCell(position);
		public bool TryGetCell(Vector2Int position, out Cell cell) => _grid.TryGetCell(position, out cell);

		#region Players
		public void SetMainPlayer(Player player, Castle castle) {
			_gameUI.MainPlayer.Bind(player);
		}
		public void SetSecondPlayer(Player player, Castle castle) {
			_gameUI.SecondPlayer.Bind(player);
		}
		public void BindSystems(Player player, Castle castle) {
			foreach (var requirePlayerMono in _playerSystems) {
				requirePlayerMono.Bind(castle, player);
			}
		}
		
		public Player AddPlayer(Castle castle) => AddPlayer(UnityEngine.Random.ColorHSV(), castle);
		public Player AddPlayer(Color color, Castle castle) => AddPlayer(new Player(color, PlayerFlags.AI), castle);
		public Player AddPlayer(Player enemy, Castle castle) {
			return enemy; // FIXME: Obsolete method
		}
		public EnemyAI AddAI(Player player, Castle castle) {
			var ai = new GameObject($"AI {player.Color.ToHex(false)}", typeof(EnemyAI)).GetComponent<EnemyAI>();
			ai.transform.parent = transform;
			ai.Init(player, this, castle);
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
		/// <summary>
		/// Sets the availability of timescale button
		/// </summary>
		public void SetTimescaleAllowed(bool allow) {
			_gameUI.SetTimescaleButton(allow);
		}
		public void SetLeaderboardAllowed(bool allow) {
			_gameUI.Leaderboard.SetEnabled(true);
		}
		public void SetCameraPosition(Vector2 position) {
			var position3d = (Vector3)position;
			position3d.z = -10;
			_cameraMovement.transform.position = position3d;
		}
		#endregion

		public static void TickAll(int ticks) {
			Tick?.Invoke(ticks);
		}
		
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