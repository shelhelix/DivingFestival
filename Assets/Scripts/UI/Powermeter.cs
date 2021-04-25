using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI {
	public class Powermeter : GameplayComponent {
		[NotNull] public TMP_Text DepthText;

		Submarine _submarine;
		
		public override void Init(GameplayStarter starter) {
			starter.Submarine.CurPower.OnValueChanged += UpdateText;
		}

		void UpdateText(float value) {
			var secondLeft = value / Submarine.TotalEnergyUnits;
			DepthText.text = secondLeft.ToString("0.");
		}
	}
}