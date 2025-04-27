using System;
using System.Collections.Generic;
using Core;
using Game.Inputs;
using Game.Tiles;
using Game.Tiles.Levels;
using UnityEngine;

namespace Game.Testing {
	public class NewLevelEditor: MonoBehaviour {
		[SerializeField] private LevelRoot _root;
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private Player[] _players;
		[SerializeField] private ClickInput _clickInput;
		private readonly List<Cell> _cells = new List<Cell>();

		private int _selectedPlayer = 0;
		private Camera _camera;

		private void Awake() {
			_camera = Camera.main;
		}
		private void Start() {
			_root.SetFreeCameraAllowed(true);
		}
		private void Update() {
			var position = GetCellUnderMouse();
			/*
			 if (Input.GetKeyDown(KeyCode.Mouse0)) {
				var cell = _root.PlaceEmptyCell(position);
				_cells.Add(cell);
			}
			*/
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
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				_root.AttachFence(position);
			}

			if (Input.GetKeyDown(KeyCode.Q)) {
				_selectedPlayer = Mathf.Clamp(_selectedPlayer - 1, 0, _players.Length - 1);
			}
			if (Input.GetKeyDown(KeyCode.E)) {
				_selectedPlayer = Mathf.Clamp(_selectedPlayer + 1, 0, _players.Length - 1);
			}
			if (Input.GetKeyDown(KeyCode.W)) {
				if (_grid.TryGetCell(position, out var cell)) {
					cell.Capture(_players[_selectedPlayer]);
				}
			}
		}
		private void Place() {
			var position = GetCellUnderMouse();
			var cell = _root.PlaceEmptyCell(position);
			_cells.Add(cell);
		}
		
		public SerializedLevel GetLevel() {
			return SerializedLevel.Create(_cells.ToArray());
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
			style.normal.textColor = _players[_selectedPlayer].Color;
			GUI.Label(rect, $"Player [{_selectedPlayer}]: {_players[_selectedPlayer].Color.ToHex()}", style);
		}

		private void OnEnable() {
			_clickInput.Performed += Place;
		}
		private void OnDisable() {
			_clickInput.Performed -= Place;
		}
	}
}