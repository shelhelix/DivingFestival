using GameComponentAttributes;

namespace LD48Project.Common {
	public class BaseStarter : GameComponent {
		protected override void Awake() {
			base.Awake();
			GameController.Instance.Activate();
		}
	}
}