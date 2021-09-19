using GameComponentAttributes;
using LD48Project.Utils.Editor.Build;
using LD48Project.Utils.GameVersioning;
using UnityEngine;

namespace LD48Project.Utils {
	public class NotMobileObjectHider : GameComponent {
		public void Start() {
			gameObject.SetActive(Application.isMobilePlatform);
		}
	}
}