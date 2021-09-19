using GameComponentAttributes;
using LD48Project.Utils.GameVersioning;

namespace LD48Project.Utils {
	public class ProductionObjectHider : GameComponent {
		public void Start() {
			gameObject.SetActive(!GameVersion.Instance.IsProduction);
		}
	}
}