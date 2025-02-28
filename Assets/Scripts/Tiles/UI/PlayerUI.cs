using UnityEngine;

namespace Game.Tiles.UI {
	public class PlayerUI: MonoBehaviour {
		[SerializeField] private WalletView _strategyView;
		[SerializeField] private WalletView _logisticsView;

		private Player _player;

		public void Bind(Player player) {
			_player = player;
			_strategyView.Bind(_player.StrategyPoints);
			_logisticsView.Bind(_player.LogisticsPoints);
		}
	}
}