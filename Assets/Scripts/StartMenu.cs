using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;

namespace LD48Project {
	public class StartMenu : GameComponent {
		[NotNull] public Button StartGame;

		void Start() {
			StartGame.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));			
		}

		private void OnDestroy() {
			StartGame.onClick.RemoveAllListeners();
		}
	}
}