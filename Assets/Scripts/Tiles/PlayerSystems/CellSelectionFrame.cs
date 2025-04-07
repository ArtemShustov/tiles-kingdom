using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Tiles.PlayerSystems {
	public class CellSelectionFrame: RequirePlayerMono {
		[Header("Components")]
		[SerializeField] private Transform _container;
		[SerializeField] private TMP_Text _costLabel;
		[SerializeField] private SpriteRenderer _frame;
		[SerializeField] private PlayGrid _grid;
		private Camera _camera;
		
		private Cell _selectedCell;
		
		private void Awake() {
			_camera = Camera.main;
			_container.gameObject.SetActive(false);
		}
		private void Update() {
			if (Utils.IsPaused()) {
				return;
			}
			if (Player == null || !Castle) {
				return;
			}
			
			var cellPos = GetCellUnderMouse();
			var worldPos = _grid.GetCellCenterWorld(cellPos);
			_container.transform.position = worldPos;

			UpdateView(cellPos);
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
				_costLabel.text = cell.GetCaptureCostFor(Player).ToString();
			}

			if (cell) {
				_costLabel.gameObject.SetActive(cell.Owner.Value != Player);
				_costLabel.color = Player.StrategyPoints.CanTake(cell.GetCaptureCostFor(Player)) ? Color.white : Color.red;
			}
			_frame.color = HasPathToCastle(cellPos) ? Color.green : Color.red;
		}
		
		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _grid.WorldToCell(worldPos);
			return cellPos;
		}
		private bool HasOwnedNeighbourCell(Vector2Int position) {
			return _grid.GetNeighbours(position).Any(cell => cell.Owner.Value == Player);
		}
		private bool HasPathToCastle(Vector2Int position) {
			var cell = _grid.GetCell(position);
			var finder = new GridPathFinder(_grid);
			return finder.HasPath(cell, Castle.Cell, Player);
		}
	}
}