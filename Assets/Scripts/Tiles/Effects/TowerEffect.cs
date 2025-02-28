namespace Game.Tiles.Effects {
	public class TowerEffect: ICaptureCostEffect {
		private readonly int _captureCost;
		public Player Owner { get; set; }
		
		public TowerEffect(int cost, Player owner) {
			_captureCost = cost;
			Owner = owner;
		}

		public int GetCaptureCostFor(Player player, Cell cell) {
			if (Owner == player) {
				return 0;
			}
			return _captureCost;
		}
	}
}