namespace LD48Project.Achievements {
	public abstract class BaseAchievement {
		public abstract string AchievementName { get; }

		public abstract float GetProgress();
	}
}