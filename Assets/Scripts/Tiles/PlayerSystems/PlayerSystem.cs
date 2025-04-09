using Game.Tiles.Buildings;
using UnityEngine;

namespace Game.Tiles.PlayerSystems {
	public class PlayerSystem: MonoBehaviour {
		public Player Player { get; private set; }
		public Castle Castle { get; private set; }
		
		public bool IsBinded => Castle != null && Player != null;

		public void Bind(Castle castle, Player player) {
			Player = player;
			Castle = castle;
		}
	}
}