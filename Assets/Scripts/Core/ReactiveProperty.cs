using System;

namespace Core {
	public class ReactiveProperty<T>: IReadOnlyReactiveProperty<T> {
		private T _value;

		public event Action ValueChanged;
		
		public T Value {
			get => _value;
			set {
				_value = value;
				ValueChanged?.Invoke();
			}
		}

		public ReactiveProperty() { }
		public ReactiveProperty(T value) {
			_value = value;
		}
	}
}