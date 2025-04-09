using Core.Events;
using Core.LiteLocalization;
using Game.Inputs;
using Game.Popups;
using Game.Tiles.Events;
using Game.Tiles.UI;
using UnityEngine;

namespace Game.Tiles.PlayerSystems {
	public class PlayerBuilderWithMenu: PlayerSystem {
		[SerializeField] private LocalizedString _noPointsHint;
		[SerializeField] private LocalizedString _noPathHint;
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _failSound;
		[Space]
		[SerializeField] private PieMenu _pie;
		[SerializeField] private LevelRoot _level;
		[SerializeField] private ClickInput _input;
		private Camera _camera;
		private bool _active;
		private Vector2Int _cell;

		private void Awake() {
			_camera = Camera.main;
			_pie.HideNow();
		}
		private void Update() {
			if (!IsBinded) {
				return;
			}
			if (_active && Input.GetMouseButtonDown(0) && !Utils.IsPointerOverUIObject()) {
				_active = false;
				_pie.Hide();
			}
		}

		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _level.Grid.WorldToCell(worldPos);
			return cellPos;
		}
		private bool CanShowMenu(Cell cell) => cell.Owner.Value == Player && cell.Building.Value == null;
		private bool HasPath(Cell from) {
			var finder = new GridPathFinder(_level.Grid);
			return finder.HasPath(from, Castle.Cell, Player);
		}
		private void ShowFailHint(Vector3 worldPos, string message) {
			EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
			EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(worldPos, Color.red, message));
		}
		
		private void TryShowMenu(Vector2Int position) {
			if (!IsBinded || !_level.TryGetCell(position, out var cell) || !CanShowMenu(cell)) {
				return;
			}
			
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			if (!HasPath(cell)) {
				ShowFailHint(worldPos, _noPathHint.GetLocalized());
				return;
			}
			
			_cell = position;
			_pie.transform.position = Input.mousePosition;
			foreach (var item in _pie.Items)
				item.SetLabelColor(Player.LogisticsPoints.Value >= item.Cost ? Color.white : Color.red);

			_pie.Show();
			_active = true;
		}

		private void OnHoldPerformed() {
			if (Utils.IsPointerOverUIObject() || Utils.IsPaused()) {
				return;
			}
			TryShowMenu(GetCellUnderMouse());
		}
		private void OnSelected(PieMenuItem item) {
			if (!_active || !_level.TryGetCell(_cell, out var cell)) {
				return;
			}
			
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			if (!CanShowMenu(cell) || !HasPath(cell)) {
				ShowFailHint(worldPos, _noPathHint.GetLocalized());
			} else if (Player.LogisticsPoints.Take(item.Cost)) {
				_level.AttachBuilding(_cell, item.Prefab);
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_successSound));
				EventBus<PlayerActedEvent>.Raise(new PlayerActedEvent(PlayerActedEvent.ActionType.Build));
			} else {
				ShowFailHint(worldPos, string.Format(_noPointsHint.GetLocalized(), item.Cost));
			}
			
			_pie.Hide();
			_active = false;
		}
		private void OnEnable() {
			_input.Performed += OnHoldPerformed;
			_pie.Selected += OnSelected;
		}
		private void OnDisable() {
			_input.Performed -= OnHoldPerformed;
			_pie.Selected -= OnSelected;
		}
	}
}