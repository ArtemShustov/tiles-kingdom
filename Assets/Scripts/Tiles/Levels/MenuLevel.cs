using Core;
using Game.Tiles.Levels.Utils;
using UnityEngine;

namespace Game.Tiles.Levels {
	[CreateAssetMenu(menuName = "Levels/Menu")]
	public class MenuLevel: Level {
		[Header("Cool background")]
		[SerializeField] private bool _coolBackground = true;
		[SerializeField] private Vector2Int _size = new Vector2Int(15, 10);
		[SerializeField] private int _count = 5;
		[SerializeField] private Color[] _colors = new[] {
			Color.red,
			Color.green,
			Color.blue,
			Color.yellow,
			Color.cyan,
			Color.gray,
			Color.magenta
		};
		[Header("Settings")]
		[SerializeField] private MenuCanvas _menu;
		[SerializeField] private Level _soloLevel;
		[SerializeField] private Level _duelLevel;
		
		public override void Build(LevelRoot root) {
			var menu = Instantiate(_menu, root.transform);
			menu.SoloLevel = _soloLevel;
			menu.DuelLevel = _duelLevel;
			root.SetTimescaleAllowed(false);

			if (_coolBackground) {
				BuildCoolBackground(root);
			}
		}
		private void BuildCoolBackground(LevelRoot root) {
			root.gameObject.AddComponent<RealTimeTicker>();
				
			for (int x = -_size.x; x < _size.x; x++) {
				for (int y = -_size.y; y < _size.y; y++) {
					root.PlaceEmptyCell(new Vector2Int(x, y));
				}
			}

			for (int i = 0; i < _count; i++) {
				var pos = new Vector2Int(
					Random.Range(-_size.x, _size.x),
					Random.Range(-_size.y, _size.y)
				);
				if (root.Grid.TryGetCell(pos, out var cell)) {
					if (cell.Building.Value) {
						continue;
					}
				}
				var color = i >= _colors.Length ? _colors.GetRandom() : _colors[i];
				var player = new Player(color, PlayerFlags.AI);
				var castle = root.AttachCastle(pos);
				castle.Cell.Capture(player);
				root.AddAI(player, castle);
				player.StrategyPoints.Add(1000);
			}

			for (int i = 0; i < _count; i++) {
				var pos = new Vector2Int(
					Random.Range(-_size.x, _size.x),
					Random.Range(-_size.y, _size.y)
				);
				if (root.Grid.TryGetCell(pos, out var cell)) {
					if (cell.Building.Value) {
						continue;
					}
				}
				root.AttachMine(pos);
			}
			
			for (int i = 0; i < _count; i++) {
				var pos = new Vector2Int(
					Random.Range(-_size.x, _size.x),
					Random.Range(-_size.y, _size.y)
				);
				if (root.Grid.TryGetCell(pos, out var cell)) {
					if (cell.Building.Value) {
						continue;
					}
				}
				root.AttachTower(pos);
			}
		}
	}
}