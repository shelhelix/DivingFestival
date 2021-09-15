using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LD48Project.Utils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate();
			PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, status => {
				IsLoggedIn = status == SignInStatus.Success;
				if ( !IsLoggedIn ) {
					LoginFailReason = status;
				}
			} );
		}

		public async UniTask<LeaderboardScoreData> RequestPlayerCentricHighScoreTableAsync(string tableId) {
			if ( !_isInited ) {
				return null;
			}
			await UniTask.SwitchToThreadPool();
			LeaderboardScoreData data = null;
			PlayGamesPlatform.Instance.LoadScores(tableId, LeaderboardStart.PlayerCentered, 10, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
				(x) => {
					data = x;
				});
			await UniTask.WaitWhile(() => data == null);
			return data;
		}


		public async UniTask PublishScoreAsync(string highScoreTableId, long score) {
			if ( !IsLoggedIn ) {
				return;
			}
			await UniTask.SwitchToThreadPool();
			var completed = false;
			PlayGamesPlatform.Instance.ReportScore(score, highScoreTableId, (x) => {
				completed = true;
			});
			await UniTask.WaitWhile(() => !completed);
		}

		public async UniTask<IUserProfile[]> GetUserNamesAsync(string[] userIds) {
			IUserProfile[] result      = null;
			PlayGamesPlatform.Instance.LoadUsers(userIds, (x) => {
				result      = x;
			});
			await UniTask.WaitWhile(() => result == null);
			return result;
		}

		public void SetProgressToAchievement(string achievementId, float progressPercent) {
			if ( !IsLoggedIn ) {
				return;
			}
			Social.ReportProgress(achievementId, progressPercent, null);
		}
	}
}