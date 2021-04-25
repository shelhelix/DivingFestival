using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI {
	public class Depthmeter : GameplayComponent {
		[NotNull] public TMP_Text DepthText;
		public override void Init(GameplayStarter starter) {
			starter.Submarine.Depth.OnValueChanged += UpdateDepthText;
		}

		void UpdateDepthText(float depth) {
			DepthText.text = depth.ToString(".0");
		}
	}
}