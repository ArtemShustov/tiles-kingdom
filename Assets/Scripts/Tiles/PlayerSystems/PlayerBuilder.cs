using System;
using Core.Events;
using Core.LiteLocalization;
using Game.Inputs;
using Game.Popups;
using Game.Tiles.Events;
using UnityEngine;

namespace Game.Tiles.PlayerSystems {
	[Obsolete("Use PlayerBuilderWithMenu instead")]
	public class PlayerBuilder: PlayerSystem {
		[SerializeField] private int _cost = 3;
		[SerializeField] private LocalizedString _noPointsHint;
		[SerializeField] private LocalizedString _noPathHint;
		[Space]
		[SerializeField] private LevelRoot _level;
		[SerializeField] private AudioClip _successSound;
		[SerializeField] private AudioClip _failSound;
		[SerializeField] private HoldInput _input;
		private Camera _camera;

		private void Awake() {
			_camera = Camera.main;
		}
		private void Update() {
			if (!IsBinded) {
				return;
			}
			
			#if DEBUG
			if (Input.GetKeyDown(KeyCode.L)) {
				Player.LogisticsPoints.Add(100);
			}
			#endif
		}

		private void Build(Vector2Int position) {
			if (!_level.TryGetCell(position, out var cell)) {
				return;
			}
			if (cell.Owner.Value != Player) {
				return;
			}
			if (cell.Building.Value != null) {
				return;
			}
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			
			var finder = new GridPathFinder(_level.Grid);
			if (!finder.HasPath(cell, Castle.Cell, Player)) {
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(
					worldPos, 
					Color.red, 
					_noPathHint.GetLocalized()
				));
				return;
			}
			
			if (Player.LogisticsPoints.Take(_cost)) {
				_level.AttachFence(position);
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_successSound));
				EventBus<PlayerActedEvent>.Raise(new PlayerActedEvent(PlayerActedEvent.ActionType.Build));
			} else {
				EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(_failSound));
				EventBus<ShowPopupEvent>.Raise(new ShowPopupEvent(
					worldPos, 
					Color.red, 
					string.Format(_noPointsHint.GetLocalized(), _cost)
				));
			}
		}
		private Vector2Int GetCellUnderMouse() {
			var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
			var cellPos = _level.Grid.WorldToCell(worldPos);
			return cellPos;
		}

		private void OnInput() {
			if (Utils.IsPointerOverUIObject() || Utils.IsPaused()) {
				return;
			}
			Build(GetCellUnderMouse());
		}
		private void OnEnable() {
			_input.Performed += OnInput;
		}
		private void OnDisable() {
			_input.Performed -= OnInput;
		}
	}
}