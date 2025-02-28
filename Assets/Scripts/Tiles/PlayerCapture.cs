using System.Linq;
using Core.Events;
using Game.Tiles.Buildings;
using Game.Tiles.Popups;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Tiles {
	public class PlayerCapture: MonoBehaviour {
		[SerializeField] private LocalizedString _noPointsHint;
		[SerializeField] private LocalizedString _noPathHint;
		[Space]
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _failSound;
		private Camera _camera;
		
		private Player _player;
		private Castle _playerCastle;
		
		private void Awake() {
			_camera = Camera.main;
		}
		private void Update() {
			if (Input.GetMouseButtonDown(0)) {
				Capture(GetCellUnderMouse());
			}
		}
		public void Bind(Castle castle, Player player) {
			_player = player;
			_playerCastle = castle;
		}

		private void Capture(Vector2Int position) {
			if (!_grid.TryGetCell(position, out var cell)) {
				return;
			}
			if (cell.Owner.Value == _player) {
				return;
			}
			/*
			if (!HasOwnedNeighbourCell(position)) {
				var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(worldPos, Color.red, $"Too far"));
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
				return;
			}
			*/
			
			var finder = new GridPathFinder(_grid);
			if (!finder.HasPath(_playerCastle.Cell, cell, _player)) {
				var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(
					worldPos, 
					Color.red, 
					_noPathHint.GetLocalizedString()
				));
				return;
			}
			
			var cost = cell.GetCaptureCostFor(_player);
			if (_player.StrategyPoints.Take(cost)) {
				cell.Capture(_player);
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_successSound));
			} else {
				var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(
					worldPos, 
					Color.red, 
					string.Format(_noPointsHint.GetLocalizedString(), cost)
				));
			}
		}
		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _grid.WorldToCell(worldPos);
			return cellPos;
		}
		private bool HasOwnedNeighbourCell(Vector2Int position) {
			return _grid.GetNeighbours(position).Any(cell => cell.Owner.Value == _player);
		}
	}
}