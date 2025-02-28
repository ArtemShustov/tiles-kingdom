using System;

namespace Core {
	public interface IReadOnlyReactiveProperty<T> {
		public event Action ValueChanged;
		
		public T Value { get; }
	}
}