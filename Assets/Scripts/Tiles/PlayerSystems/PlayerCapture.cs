using System.Linq;
using Core;
using Core.Events;
using Game.Tiles.Popups;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

namespace Game.Tiles.PlayerSystems {
	public class PlayerCapture: RequirePlayerMono {
		[SerializeField] private LocalizedString _noPointsHint;
		[SerializeField] private LocalizedString _noPathHint;
		[Space]
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _failSound;
		private Camera _camera;
		private DefaultInput _input;
		
		private void Awake() {
			_camera = Camera.main;
			_input = new DefaultInput();
		}

		private void Capture(Vector2Int position) {
			if (!_grid.TryGetCell(position, out var cell)) {
				return;
			}
			if (cell.Owner.Value == Player) {
				return;
			}
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			
			var finder = new GridPathFinder(_grid);
			if (!finder.HasPath(Castle.Cell, cell, Player)) {
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(
					worldPos, 
					Color.red, 
					_noPathHint.GetLocalizedString()
				));
				return;
			}
			
			var cost = cell.GetCaptureCostFor(Player);
			if (Player.StrategyPoints.Take(cost)) {
				cell.Capture(Player);
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_successSound));
			} else {
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
			return _grid.GetNeighbours(position).Any(cell => cell.Owner.Value == Player);
		}

		private void OnInput(InputAction.CallbackContext obj) {
			if (Utils.IsPointerOverUIObject() || Utils.IsPaused()) {
				return;
			}
			Capture(GetCellUnderMouse());
		}
		private void OnEnable() {
			_input.Player.Enable();
			_input.Player.PrimaryAction.performed += OnInput;
		}
		private void OnDisable() {
			_input.Player.Disable();
			_input.Player.PrimaryAction.performed -= OnInput;
		}
	}
}