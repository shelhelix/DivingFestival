using LD48Project.Starter;

using GameComponentAttributes.Attributes;

namespace LD48Project.UI.PowerPanel {
	public class PowerItem : GameplayComponent {
		public Submarine.Subsystem Subsystem;

		[NotNull] public BasePowerBar Bar;

		public override void Init(GameplayStarter starter) {
			starter.Submarine.EnergyDistribution[Subsystem].OnValueChanged += OnValueChanged;
			OnValueChanged(starter.Submarine.EnergyDistribution[Subsystem].Value);
		}

		void OnValueChanged(int value) {
			Bar.SetValue(value);
		}
	}
}