namespace Game.Tiles.Effects {
	public interface ICaptureCostEffect: ICellEffect {
		int GetCaptureCostFor(Player player, Cell cell);
	}
}