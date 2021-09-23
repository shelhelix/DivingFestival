#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.SocialPlatforms;

using LD48Project.Utils;

using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

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

		public async UniTask<LeaderboardScoreData> RequestPlayerCentricHighScoreTableAsync(string tableId, int recordsCount) {
			if ( !_isInited ) {
				return null;
			}
			LeaderboardScoreData data = null;
			PlayGamesPlatform.Instance.LoadScores(tableId, LeaderboardStart.PlayerCentered, recordsCount, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
				(x) => {
					data = x;
				});
			await UniTask.WaitWhile(() => data == null);
			return data;
		}

		public void SetProgressToAchievement(string achievementId, float progressPercent) {
			if ( !IsLoggedIn ) {
				return;
			}
			Social.ReportProgress(achievementId, progressPercent, null);
		}
	}
}
#endif