using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LD48Project.ExternalServices.Leaderboards {
	public class StubLeaderboardService : ILeaderboardService {
		public UniTask PublishScoreAsync(long score) {
			return UniTask.CompletedTask;
		}

		public async UniTask<List<Score>> GetScoresAroundPlayer(int recordsCount) {
			var res = new List<Score>();
			return res;
		}
	}
}