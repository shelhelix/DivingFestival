using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.Common;
using LD48Project.ExternalServices;
using LD48Project.ExternalServices.Ads;
using LD48Project.Leaderboards;
using LD48Project.UI.StartScene;
using UnityEngine;

namespace LD48Project {
	public class StartSceneStarter : BaseStarter {
		[NotNull] public Button                    StartGame;
		[NotNull] public SetUserNameDialogueWindow SetUserNameDialogueWindow;
		
		void Start() {
			StartGame.onClick.AddListener(() => SceneManager.LoadScene("Gameplay"));
			
			var leaderboardController = GameController.Instance.LeaderboardController;
			SetUserNameDialogueWindow.Init(leaderboardController, () => {
				StartGame.gameObject.SetActive(true);
			});

			if ( !leaderboardController.HasStoredUser ) {
				StartGame.gameObject.SetActive(false);
				SetUserNameDialogueWindow.Show();
			}
		}
	}
}