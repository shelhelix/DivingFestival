using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using UnityEngine;

namespace LD48Project {
	public class StartMenu : GameComponent {
		[NotNull] public Button StartGame;
		[NotNull] public Button OpenHowToScreenButton;
		[Header("how to")] 
		[NotNull] public Button ReturnToMainMenuButton;

		[NotNull] public GameObject MainScreenRoot;
		[NotNull] public GameObject HowToScreenRoot;
		
		void Start() {
			StartGame.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));			
			OpenHowToScreenButton.onClick.AddListener(() => {
				MainScreenRoot.SetActive(false);
				HowToScreenRoot.SetActive(true);
			});
			ReturnToMainMenuButton.onClick.AddListener(() => {
				MainScreenRoot.SetActive(true);
				HowToScreenRoot.SetActive(false);
			});
		}

		void OnDestroy() {
			StartGame.onClick.RemoveAllListeners();
			OpenHowToScreenButton.onClick.RemoveAllListeners();
			ReturnToMainMenuButton.onClick.RemoveAllListeners();
		}
	}
}