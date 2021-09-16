using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GooglePlayGames.BasicApi;
using LD48Project.Utils;
using UnityEngine;

namespace LD48Project.ExternalServices {
	public readonly struct Score {
		public readonly int    Rank;
		public readonly long   ScoreValue;
		public readonly string UserName;

		public Score(int rank, long score, string userName) {
			Rank       = rank;
			ScoreValue = score;
			UserName   = userName;
		}
	}

	public class LeaderboardService : Singleton<LeaderboardService> {

		public UniTask PublishScore(long score) {
			return GooglePlayGamesService.Instance.PublishScoreAsync(GPGSIds.leaderboard_max_depth, score);
		}

		public async UniTask<List<Score>> GetScoresAroundPlayer(int recordsCount) {
			var res = new List<Score>();
			
			if ( Application.isEditor ) {
				for( var i = 0; i < recordsCount; i++ ) {
					res.Add(new Score(i, (recordsCount - i) * 100, $"test user {i}"));
				}
				return res;
			}
			
			var leaderboard = await GooglePlayGamesService.Instance.RequestPlayerCentricHighScoreTableAsync(GPGSIds.leaderboard_max_depth, recordsCount);
			if ( !leaderboard.Valid ) {
				return null;
			}
			
			var idToNameConverter = await GetUserIdToUserNameConverted(GooglePlayGamesService.Instance, leaderboard);
			foreach ( var score in leaderboard.Scores ) {
				var userName = idToNameConverter[score.userID];
				res.Add(new Score(score.rank, score.value, userName));
			}
			return res;
		}

		async UniTask<Dictionary<string, string>> GetUserIdToUserNameConverted(GooglePlayGamesService service, LeaderboardScoreData data) {
			var res     = new Dictionary<string, string>();
			var userIds = data.Scores.Select(score => score.userID).ToArray();
			var users   = await service.GetUserNamesAsync(userIds);
			foreach ( var id in userIds ) {
				var user = users.First(x => x.id == id);
				res.Add(id, user.userName);
			}
			return res;
		}
	}
}