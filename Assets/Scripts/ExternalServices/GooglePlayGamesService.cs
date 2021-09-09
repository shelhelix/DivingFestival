using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LD48Project.Utils;
using UnityEngine;

namespace LD48Project.ExternalServices {
	public class GooglePlayGamesService : Singleton<GooglePlayGamesService> {

		bool _isLoggedIn;
		bool _isInited;

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
			PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, status => {
				_isLoggedIn = status == SignInStatus.Success;
				if ( !_isLoggedIn ) {
					Debug.LogError($"Can't log in. Reason: {status}");
				}
			} );
		}

		public void PublishScore(string highscoreTableId, long score) {
			if ( !_isLoggedIn ) {
				return;
			}
			Social.ReportScore(score, highscoreTableId, null);
		}

		public void SetProgressToAchievement(string achievementId, float progressPercent) {
			if ( !_isLoggedIn ) {
				return;
			}
			Social.ReportProgress(achievementId, progressPercent, null);
		}
	}
}