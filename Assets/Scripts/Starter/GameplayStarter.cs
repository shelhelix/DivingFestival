using System.Collections.Generic;
using GameComponentAttributes.Attributes;
using LD48Project.Common;
using LD48Project.ExternalServices;
using LD48Project.ExternalServices.Ads;
using LD48Project.Leaderboards;

namespace LD48Project.Starter {
    public class GameplayStarter : BaseStarter {
	    [NotNull] public Submarine Submarine;

		public LeaderboardController LeaderboardController => GameController.Instance.LeaderboardController;
		
	    void Start() {
			AdvertisementService.Instance.Init();
			#if UNITY_ANDROID
			GooglePlayGamesService.Instance.Init();
			AchievementService.Instance.Init(Submarine, GooglePlayGamesService.Instance);
			#endif
			
			InitComponents();
	    }

	    void InitComponents() {
		    var comps = new List<GameplayComponent>(GameplayComponent.Instances);
		    foreach ( var comp in comps ) {
			    comp.Init(this);
		    }
	    }

		void Update() {
			AchievementService.Instance.TryToReportAchievementsProgress();
		}
    }
}