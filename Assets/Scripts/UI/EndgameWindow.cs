using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using LD48Project.ExternalServices;
using LD48Project.ExternalServices.Ads;

using DG.Tweening;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using GooglePlayGames.BasicApi;
using LD48Project.ExternalServices.Leaderboards;
using TMPro;

namespace LD48Project.UI {
	public class EndgameWindow : GameComponent {
		const string DepthDescTemplate = "Your max depth is: \n{0:.0}";
		
		[NotNull] public TMP_Text    DepthDescText;
		[NotNull] public Button      ReturnToMenuButton;
		[NotNull] public Button      AddPowerAdButton;
		[NotNull] public CanvasGroup GameplayUI;

		[NotNull] public Transform CenterPoint;

		[NotNull] public LeaderBoard LeaderBoard;
		
		Vector3 _startPos;

		Submarine _submarine;

		Tween _showTween;
		
		void Update() {
			
		}
		
		public void Init(Submarine submarine) {
			GameplayUI.DOFade(0f, 0.5f);
			_showTween         = transform.DOMove(CenterPoint.position, 1f);
			_startPos          = transform.position;
			_submarine         = submarine;
			DepthDescText.text = string.Format(DepthDescTemplate, submarine.Depth.Value);
			ReturnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene("StartMenu"));
			AddPowerAdButton.onClick.AddListener(() => AdvertisementService.Instance.ShowAd(OnAd));
			AchievementService.Instance.TryToReportAchievementsProgress();
			InitLeaderboard();
			TriggerLeaderboardAnimation();
		}

		async void InitLeaderboard() {
			await LeaderboardService.Instance.PublishScore(Mathf.FloorToInt(_submarine.Depth.Value));
			LeaderBoard.Init();
		}

		async void TriggerLeaderboardAnimation() {
			LeaderBoard.PrepareForAnimation();
			await UniTask.WaitUntil(() => LeaderBoard.Inited && !_showTween.IsPlaying());
			LeaderBoard.GetShowSeq().Play();
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