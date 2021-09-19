using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LD48Project.Utils;

namespace LD48Project.ExternalServices.Leaderboards {
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
		ILeaderboardService _leaderboardService;
		
		public LeaderboardService() {
			#if UNITY_ANDROID
			_leaderboardService = AndroidLeaderboardService.Instance;
			#else
			_leaderboardService = new StubLeaderboardService();
			#endif

		}
		
		public UniTask PublishScore(long score) {
			return _leaderboardService.PublishScoreAsync(score);
		}

		public UniTask<List<Score>> GetScoresAroundPlayer(int recordsCount) {
			return _leaderboardService.GetScoresAroundPlayer(recordsCount);
		}
	}
}