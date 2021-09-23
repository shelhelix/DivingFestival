using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LD48Project.Common;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace LD48Project.Leaderboards {
	public class LeaderboardController {
		const string LeaderBoardName = "Scores";
		
		GameState    _state;

		LeaderboardState _leaderboardState => _state.LeaderboardState;

		public bool HasStoredUser => _leaderboardState.UserGuid != Guid.Empty;
		
		bool IsLoggedIn => PlayFabClientAPI.IsClientLoggedIn();

		public LeaderboardController(GameState state) {
			_state = state;
			if ( _leaderboardState.UserGuid != Guid.Empty ) {
				TryLogin().Forget();
			}
		}
		
		public async UniTask PublishScoreAsync(int score) {
			if ( !IsLoggedIn ) {
				return;
			}
			var request            = FromPublishScoreRequest(score);
			var isPublishCompleted = false;
			PlayFabClientAPI.UpdatePlayerStatistics(request, result => { isPublishCompleted = true; }, (error) => {
				Debug.LogError($"Something went wrong on pushing score: {error.ErrorMessage}");
				isPublishCompleted = true;
			});
			await UniTask.WaitWhile(() => !isPublishCompleted);
		}

		public async UniTask<List<Score>> GetScoresAroundPlayerAsync(int recordsCount) {
			if ( !IsLoggedIn ) {
				return null;
			}
			var isRequestCompleted = false;
			var request            = FormLeaderboardRequest(recordsCount);

			List<PlayerLeaderboardEntry> results = null;
			PlayFabClientAPI.GetLeaderboardAroundPlayer(request, (resultObj) => {
				results            = resultObj.Leaderboard;
				isRequestCompleted = true;
			}, (error) => {
				Debug.LogError($"Something went wrong on getting score: {error.ErrorMessage}");
				isRequestCompleted = true;
			});
			await UniTask.WaitWhile(() => !isRequestCompleted);
			return ConvertPlayFabInfoToOurFormat(results);
		}

		public void SetUserName(string userName) {
			_leaderboardState.UserName = userName;
			_leaderboardState.UserGuid = Guid.NewGuid();
		}

		public async UniTaskVoid TryLogin() {
			if ( IsLoggedIn ) {
				Debug.LogError("You are already logged in.");
				return;
			}
			var request            = FormLoginRequest();
			var isRequestCompleted = false;
			PlayFabClientAPI.LoginWithCustomID(request, (result) => { isRequestCompleted = true; }, (error) => {
				isRequestCompleted = true;
				Debug.LogError($"Login failed. Reason: {error.ErrorMessage}");
			} );
			await UniTask.WaitWhile(() => !isRequestCompleted);
		}

		List<Score> ConvertPlayFabInfoToOurFormat(List<PlayerLeaderboardEntry> scores) {
			return scores?.Select(score => new Score(score.Position, score.StatValue, score.DisplayName)).ToList();
		}

		LoginWithCustomIDRequest FormLoginRequest() {
			return new LoginWithCustomIDRequest{
				CustomId      = _leaderboardState.UserGuid.ToString(),
				CreateAccount = true
			};
		}
		
		GetLeaderboardAroundPlayerRequest FormLeaderboardRequest(int maxRecordsCount) {
			return new GetLeaderboardAroundPlayerRequest {
				StatisticName   = LeaderBoardName,
				MaxResultsCount = maxRecordsCount
			};
		}

		UpdatePlayerStatisticsRequest FromPublishScoreRequest(int score) {
			return new UpdatePlayerStatisticsRequest {
				Statistics = new List<StatisticUpdate> {
					new StatisticUpdate {
						StatisticName = LeaderBoardName,
						Value         = score
					}
				}
			};
		}
	}
}