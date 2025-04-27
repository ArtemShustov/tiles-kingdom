using System;
using UnityEngine;

namespace Game.Tiles {
	[Serializable]
	public class Player {
		[field: SerializeField]
		public Color Color { get; private set; }
		public Wallet StrategyPoints { get; private set; } = new();
		public Wallet LogisticsPoints { get; private set; } = new();

		public PlayerFlags Flags { get; private set; }
		
		public Player(Color color) {
			Color = color;
		}
		public Player(Color color, PlayerFlags flags) {
			Color = color;
			Flags = flags;
		}

		public bool HasFlag(PlayerFlags flag) {
			return Flags.HasFlag(flag);
		}
	}
	
	[Flags]
	public enum PlayerFlags {
		None = 0,
		Human = 1 << 0,
		AI = 1 << 1, 
		CanBuild = 1 << 2,
		Cheating = 1 << 3,
	}
}