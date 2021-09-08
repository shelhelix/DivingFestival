using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LD48Project.Utils;
using UnityEngine;

namespace LD48Project.ExternalServices {
	public class GooglePlayGamesService : Singleton<GooglePlayGamesService> {

		bool _isLoggedIn;
		bool _isInited;

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
				Debug.LogError("Can't report score - user not logged in");
				return;
			}
			Social.ReportScore(score, highscoreTableId, null);
		}

		public void SetProgressToAchievement(string achievementId, float progressPercent) {
			if ( !_isLoggedIn ) {
				Debug.LogError("Can't set progress to the achievement - user not logged in");
				return;
			}
			Social.ReportProgress(achievementId, progressPercent, null);
		}
	}
}