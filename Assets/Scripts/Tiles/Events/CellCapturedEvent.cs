using Core.Events;

namespace Game.Tiles.Events {
	public readonly struct CellCapturedEvent: IGameEvent {
		public Cell Cell { get; }
		public Player PreviousOwner { get; }
		public Player By { get; }

		public CellCapturedEvent(Cell cell, Player previousOwner, Player by) {
			Cell = cell;
			PreviousOwner = previousOwner;
			By = by;
		}
	}
}