using System.Linq;
using Game.Tiles.Buildings;
using TMPro;
using UnityEngine;

namespace Game.Tiles {
	public class CellSelectionFrame: MonoBehaviour {
		[Header("Components")]
		[SerializeField] private Transform _container;
		[SerializeField] private TMP_Text _costLabel;
		[SerializeField] private SpriteRenderer _frame;
		[SerializeField] private PlayGrid _grid;
		private Camera _camera;
		private Player _player;
		private Castle _playerCastle;
		
		private Cell _selectedCell;
		
		private void Awake() {
			_camera = Camera.main;
			_container.gameObject.SetActive(false);
		}
		private void Update() {
			if (_player == null || !_playerCastle) {
				return;
			}
			
			var cellPos = GetCellUnderMouse();
			var worldPos = _grid.GetCellCenterWorld(cellPos);
			_container.transform.position = worldPos;

			UpdateView(cellPos);
		}
		public void Bind(Castle castle, Player player) {
			_player = player;
			_playerCastle = castle;
		}
		
		private void UpdateView(Vector2Int cellPos) {
			var cell = _grid.GetCell(cellPos);
			if (cell != _selectedCell) {
				if (!cell) {
					_selectedCell = null;
					_container.gameObject.SetActive(false);
					return;
				}
				if (!_selectedCell) {
					_container.gameObject.SetActive(true);
				}
				_selectedCell = cell;
				_costLabel.text = cell.GetCaptureCostFor(_player).ToString();
			}

			if (cell) {
				_costLabel.color = _player.StrategyPoints.CanTake(cell.GetCaptureCostFor(_player)) ? Color.white : Color.red;
			}
			_frame.color = HasPathToCastle(cellPos) ? Color.green : Color.red;
		}
		
		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _grid.WorldToCell(worldPos);
			return cellPos;
		}
		private bool HasOwnedNeighbourCell(Vector2Int position) {
			return _grid.GetNeighbours(position).Any(cell => cell.Owner.Value == _player);
		}
		private bool HasPathToCastle(Vector2Int position) {
			var cell = _grid.GetCell(position);
			var finder = new GridPathFinder(_grid);
			return finder.HasPath(cell, _playerCastle.Cell, _player);
		}
	}
}