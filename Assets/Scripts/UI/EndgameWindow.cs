using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.ExternalServices;
using LD48Project.ExternalServices.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

namespace LD48Project.UI {
	public class EndgameWindow : GameComponent {
		const string DepthDescTemplate = "Your max depth is: \n{0:.0}";
		
		[NotNull] public TMP_Text    DepthDescText;
		[NotNull] public Button      ReturnToMenuButton;
		[NotNull] public Button      AddPowerAdButton;
		[NotNull] public CanvasGroup GameplayUI;

		[NotNull] public Transform CenterPoint;

		[NotNull] public HighScoreUI HighScoreUI;
		
		Vector3 _startPos;

		public void Init(Submarine submarine, float depth) {
			_startPos          = transform.position;
			DepthDescText.text = string.Format(DepthDescTemplate, depth);
			ReturnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene("StartMenu"));
			GameplayUI.DOFade(0f, 0.5f);
			transform.DOMove(CenterPoint.position, 1f);
			AddPowerAdButton.onClick.AddListener(() => AdvertisementService.Instance.ShowAd(x => OnAd(x, submarine)));
			AchievementService.Instance.TryToReportAchievementsProgress();
			GooglePlayGamesService.Instance.PublishScore(GPGSIds.leaderboard_max_depth, Mathf.FloorToInt(submarine.Depth.Value));
			var highScoreTable = GooglePlayGamesService.Instance.RequestPlayerCentricHighScoreTable(GPGSIds.leaderboard_max_depth).Result;
			HighScoreUI.Init(highScoreTable);
		}

		void OnAd(bool success, Submarine submarine) {
			if ( !success ) {
				return;
			}
			submarine.RestoreSubmarine();
			GameplayUI.DOFade(1f, 0.5f);
			transform.DOMove(_startPos, 0.5f);
		}

		void OnDestroy() {
			ReturnToMenuButton.onClick.RemoveAllListeners();
		}
	}
}