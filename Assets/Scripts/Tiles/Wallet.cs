using System;

namespace Game.Tiles {
	public class Wallet {
		private int _value;

		public event Action ValueChanged;
		
		public int Value => _value;

		public bool CanTake(int value) {
			return _value >= value;
		}
		public bool Take(int value) {
			if (!CanTake(value)) {
				return false;
			}
			_value -= value;
			ValueChanged?.Invoke();
			return true;
		}
		public void Add(int value) {
			_value += value;
			ValueChanged?.Invoke();
		}
	}
}