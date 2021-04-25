using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI.PowerPanel {
	public class PowerItem : BasePowerItem {
		public Submarine.Subsystem Subsystem;
		
		[NotNull] public TMP_Text ControlUp;
		[NotNull] public TMP_Text ControlDown;

		public override void Init(GameplayStarter starter) {
			starter.Submarine.EnergyDistribution[Subsystem].OnValueChanged += OnValueChanged;
			ControlUp.text = starter.Submarine.SubsystemsControls[Subsystem].up;
			ControlDown.text = starter.Submarine.SubsystemsControls[Subsystem].down;
			OnValueChanged(starter.Submarine.EnergyDistribution[Subsystem].Value);
		}

		void OnValueChanged(int value) {
			Bar.SetValue(value);
		}
	}
}