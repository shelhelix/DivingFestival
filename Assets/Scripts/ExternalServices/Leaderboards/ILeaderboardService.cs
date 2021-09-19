using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace LD48Project.ExternalServices.Leaderboards {
	public interface ILeaderboardService {
		UniTask PublishScoreAsync(long score);

		UniTask<List<Score>> GetScoresAroundPlayer(int recordsCount);
	}
}