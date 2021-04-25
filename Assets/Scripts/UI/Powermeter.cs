using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI {
	public class Powermeter : GameplayComponent {
		[NotNull] public TMP_Text DepthText;
		public override void Init(GameplayStarter starter) {
			starter.Submarine.CurPower.OnValueChanged += UpdateText;
		}

		void UpdateText(float depth) {
			DepthText.text = depth.ToString(".0");
		}
	}
}