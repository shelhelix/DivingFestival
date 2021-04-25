using System;

namespace LD48Project.Utils {
	public class ReactiveValue<T> {
		T _value;

		public event Action<T> OnValueChanged;

		public ReactiveValue() { }

		public ReactiveValue(T value) {
			_value = value;
		}
		
		public T Value {
			get => _value;
			set {
				if ( value.Equals(_value) ) {
					return;
				}
				_value = value;
				OnValueChanged?.Invoke(_value);
			}
		}
		
	}
}