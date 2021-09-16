using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.ExternalServices;
using LD48Project.ExternalServices.Ads;
using UnityEngine;

namespace LD48Project {
	public class StartMenu : GameComponent {
		[NotNull] public Button StartGame;
		
		void Start() {
			// Init external services
			AdvertisementService.Instance.Init();
			GooglePlayGamesService.Instance.Init();
			
			StartGame.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));			
		}

		void OnDestroy() {
			StartGame.onClick.RemoveAllListeners();
		}
	}
}