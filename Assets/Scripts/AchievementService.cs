using System.Collections.Generic;

using LD48Project.Achievements;
using LD48Project.ExternalServices;
using LD48Project.Utils;
using UnityEngine;

namespace LD48Project {
	public class AchievementService : Singleton<AchievementService> {
#if UNITY_ANDROID
		GooglePlayGamesService _googlePlayGamesService;

		List<BaseAchievement> _achievements;

		readonly Dictionary<BaseAchievement, float> _lastProgress = new Dictionary<BaseAchievement, float>();

		bool IsInited => _googlePlayGamesService != null;
		
		public void Init(Submarine submarine, GooglePlayGamesService googleService) {
			_googlePlayGamesService = googleService;
			_achievements = new List<BaseAchievement> {
				new DepthAchievement(30, submarine.Depth)
			};

		}
#endif

		public void TryToReportAchievementsProgress() {
#if UNITY_ANDROID
			if ( !IsInited ) {
				return;
			}
			foreach ( var achievement in _achievements ) {
				_lastProgress.TryGetValue(achievement, out var lastProgress);
				var newProgress = achievement.GetProgress();
				if ( Mathf.Approximately(newProgress, lastProgress) ) {
					continue;
				}
				_lastProgress[achievement] = newProgress;
				_googlePlayGamesService.SetProgressToAchievement(achievement.AchievementName, newProgress);
			}
#endif
		}
	}
}