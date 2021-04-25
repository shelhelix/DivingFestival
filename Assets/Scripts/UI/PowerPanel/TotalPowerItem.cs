using System;

using LD48Project.Starter;

namespace LD48Project.UI.PowerPanel {
	public class TotalPowerItem : BasePowerItem {
		Submarine _submarine;
		public override void Init(GameplayStarter starter) {
			_submarine = starter.Submarine;

			foreach ( Submarine.Subsystem subsystem in Enum.GetValues(typeof(Submarine.Subsystem)) ) {
				_submarine.EnergyDistribution[subsystem].OnValueChanged += (_) => OnAnyValueChanged();
			}
			OnAnyValueChanged();
		}

		void OnAnyValueChanged() {
			Bar.SetValue(_submarine.TotalUsedPower);
		}
	}
}