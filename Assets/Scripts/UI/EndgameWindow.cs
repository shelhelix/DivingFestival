using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using LD48Project.ExternalServices;
using LD48Project.ExternalServices.Ads;

using DG.Tweening;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;

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

		GooglePlayGamesService _playGamesService;

		Submarine _submarine;

		public void Init(Submarine submarine) {
			_playGamesService  = GooglePlayGamesService.Instance;
			_startPos          = transform.position;
			_submarine         = submarine;
			DepthDescText.text = string.Format(DepthDescTemplate, submarine.Depth.Value);
			ReturnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene("StartMenu"));
			GameplayUI.DOFade(0f, 0.5f);
			transform.DOMove(CenterPoint.position, 1f);
			AddPowerAdButton.onClick.AddListener(() => AdvertisementService.Instance.ShowAd(OnAd));
			AchievementService.Instance.TryToReportAchievementsProgress();
			InitLeaderboard();
		}

		async void InitLeaderboard() {
			await _playGamesService.PublishScoreAsync(GPGSIds.leaderboard_max_depth, Mathf.FloorToInt(_submarine.Depth.Value));
			var leaderboard = await GooglePlayGamesService.Instance.RequestPlayerCentricHighScoreTableAsync(GPGSIds.leaderboard_max_depth);
			HighScoreUI.Init(leaderboard);
		}

		void OnAd(bool success) {
			if ( !success ) {
				return;
			}
			_submarine.RestoreSubmarine();
			GameplayUI.DOFade(1f, 0.5f);
			transform.DOMove(_startPos, 0.5f);
		}

		void OnDestroy() {
			ReturnToMenuButton.onClick.RemoveAllListeners();
		}
	}
}