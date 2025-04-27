using System.Collections.Generic;
using Core;
using Game.Tiles;
using Game.Tiles.Levels;
using UnityEngine;

namespace Game.Testing {
	public class LevelEditor: MonoBehaviour {
		[SerializeField] private LevelRoot _root;
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private Player _player;
		[SerializeField] private Player[] _enemies;
		private readonly List<Cell> _cells = new List<Cell>();

		private int _selectedEnemy = 0;
		private Camera _camera;

		private void Awake() {
			_camera = Camera.main;
		}

		private void Update() {
			var position = GetCellUnderMouse();
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				var cell = _root.PlaceEmptyCell(position);
				_cells.Add(cell);
			}
			if (Input.GetKeyDown(KeyCode.Mouse1)) {
				if (_grid.TryGetCell(position, out var cell)) {
					_grid.RemoveCell(position);
					_cells.Remove(cell);
					Destroy(cell.gameObject);
				}
			}

			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				_root.AttachCastle(position);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				_root.AttachMine(position);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				_root.AttachTower(position);
			}

			if (Input.GetKeyDown(KeyCode.Q)) {
				_selectedEnemy = Mathf.Clamp(_selectedEnemy - 1, 0, _enemies.Length - 1);
			}
			if (Input.GetKeyDown(KeyCode.E)) {
				_selectedEnemy = Mathf.Clamp(_selectedEnemy + 1, 0, _enemies.Length - 1);
			}
			if (Input.GetKeyDown(KeyCode.W)) {
				if (_grid.TryGetCell(position, out var cell)) {
					cell.Capture(_enemies[_selectedEnemy]);
				}
			}
			if (Input.GetKeyDown(KeyCode.S)) {
				if (_grid.TryGetCell(position, out var cell)) {
					cell.Capture(_player);
				}
			}
		}
		
		public LegacySerializedLevel GetLevel() {
			return LegacySerializedLevel.Create(_cells.ToArray(), _player.Color);
		}
		
		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _grid.WorldToCell(worldPos);
			return cellPos;
		}

		private void OnDrawGizmos() {
			if (!_camera) {
				return;
			}
			var cell = GetCellUnderMouse();
			var position = _grid.GetCellCenterWorld(cell);
			Gizmos.color = _grid.HasCell(cell) ? Color.red : Color.white;
			Gizmos.DrawWireCube(position, Vector3.one);
		}
		private void OnGUI() {
			var rect = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0, 0);
			var style = new GUIStyle();
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = 48;
			style.normal.textColor = _enemies[_selectedEnemy].Color;
			GUI.Label(rect, $"Enemy: {_enemies[_selectedEnemy].Color.ToHex()}", style);
		}
	}
}