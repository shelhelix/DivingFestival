using LD48Project.Achievements;
using LD48Project.Utils;
using UnityEngine;

namespace LD48Project {
	public class DepthAchievement : BaseAchievement {
		readonly float _targetValue;

		readonly ReactiveValue<float> _curDepth;

		public override string AchievementName => GPGSIds.achievement_first_30_meters;

		public DepthAchievement(float targetDepth, ReactiveValue<float> curDepth) {
			_targetValue = targetDepth;
			_curDepth    = curDepth;
		}

		public override float GetProgress() {
			return Mathf.Clamp(_curDepth.Value / _targetValue * 100f, 0f, 100f);
		}
	}
}