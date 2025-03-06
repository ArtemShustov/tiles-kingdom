using Game.Tiles;
using UnityEngine;

namespace Game {
	public class Cheats: MonoBehaviour {
		[SerializeField] private LevelRoot _root;

		public void AddPoints() {
			_root.Player.LogisticsPoints.Add(1);
			_root.Player.StrategyPoints.Add(1);
		}
		public void ClearPlayerPrefs() {
			PlayerPrefs.DeleteAll();
		}
	}
}