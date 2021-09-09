using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LD48Project.Utils;
using UnityEngine;

namespace LD48Project.ExternalServices {
	public class GooglePlayGamesService : Singleton<GooglePlayGamesService> {
		bool _isInited;
		
		public bool   IsLoggedIn      { get; private set;  }
		public object LoginFailReason { get; private set; }

		public GooglePlayGamesService() {
			Init();
		}

		public void Init() {
			if ( _isInited ) {
				return;
			}
			_isInited                         = true;
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, status => {
				IsLoggedIn = status == SignInStatus.Success;
				if ( !IsLoggedIn ) {
					LoginFailReason = status;
				}
			} );
		}

		public async Task<LeaderboardScoreData> RequestPlayerCentricHighScoreTable(string tableId) {
			if ( !_isInited ) {
				return null;
			}
			LeaderboardScoreData data = null;
			PlayGamesPlatform.Instance.LoadScores(tableId, LeaderboardStart.PlayerCentered, 10, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
				(x) => {
					data = x;
					foreach ( var score in x.Scores ) {
						Debug.LogWarning($"UserId: {score.userID} Score: {score.value}");
					}
				});
			while ( data == null ) {
				await UniTask.Yield();
			}
			return data;
		}


		public void PublishScore(string highscoreTableId, long score) {
			if ( !IsLoggedIn ) {
				return;
			}
			Social.ReportScore(score, highscoreTableId, null);
		}

		public void SetProgressToAchievement(string achievementId, float progressPercent) {
			if ( !IsLoggedIn ) {
				return;
			}
			Social.ReportProgress(achievementId, progressPercent, null);
		}
	}
}