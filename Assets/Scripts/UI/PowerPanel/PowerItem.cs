using GameComponentAttributes;
using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI.PowerPanel {
	public class PowerItem : GameplayComponent {
		public Submarine.Subsystem Subsystem;
		
		[NotNull] public PowerBar Bar;
		[NotNull] public TMP_Text ControlUp;

		public override void Init(GameplayStarter starter) {
			starter.Submarine.EnergyDistribution[Subsystem].OnValueChanged += OnValueChanged;
			ControlUp.text = starter.Submarine.SubsystemsControls[Subsystem];
			OnValueChanged(starter.Submarine.EnergyDistribution[Subsystem].Value);
		}

		void OnValueChanged(int value) {
			Bar.SetValue(value);
		}
	}
}