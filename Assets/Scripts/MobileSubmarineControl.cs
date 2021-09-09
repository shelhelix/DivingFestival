using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using UnityEngine.UI;

namespace LD48Project {
	public class MobileSubmarineControl : GameComponent {
		[NotNull] public Button EngineButton;
		[NotNull] public Button LeftShieldButton;
		[NotNull] public Button RightShieldButton;

		public void Init(Submarine submarine) {
			EngineButton.onClick.AddListener(() => submarine.ControlPower(Submarine.Subsystem.Engine));
			LeftShieldButton.onClick.AddListener(() => submarine.ControlPower(Submarine.Subsystem.LeftShield));
			RightShieldButton.onClick.AddListener(() => submarine.ControlPower(Submarine.Subsystem.RightShield));
		}

		public void Deinit() {
			EngineButton.onClick.RemoveAllListeners();
			LeftShieldButton.onClick.RemoveAllListeners();
			RightShieldButton.onClick.RemoveAllListeners();			
		}
	}
}
