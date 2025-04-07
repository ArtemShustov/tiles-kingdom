using System.Linq;
using System.Threading.Tasks;
using Core.Events;
using Core.LiteLocalization;
using Game.Inputs;
using Game.Popups;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.PlayerSystems {
	public class PlayerCapture: RequirePlayerMono {
		[SerializeField] private LocalizedString _noPointsHint;
		[SerializeField] private LocalizedString _noPathHint;
		[Space]
		[SerializeField] private PlayGrid _grid;
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _failSound;
		[SerializeField] private ClickInput _input;
		private Camera _camera;
		
		private void Awake() {
			_camera = Camera.main;
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
					_noPathHint.GetLocalized()
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
					string.Format(_noPointsHint.GetLocalized(), cost)
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

		private void OnInput() {
			if (Utils.IsPointerOverUIObject() || Utils.IsPaused()) {
				return;
			}
			// Capture(GetCellUnderMouse());
			CaptureDelayedAsync(GetCellUnderMouse()).Forget(); // FIXME: Temporary fix
		}
		private async Task CaptureDelayedAsync(Vector2Int position) {
			await Awaitable.NextFrameAsync();
			Capture(position);
		}
		private void OnEnable() {
			_input.Performed += OnInput;
		}
		private void OnDisable() {
			_input.Performed -= OnInput;
		}
	}
}