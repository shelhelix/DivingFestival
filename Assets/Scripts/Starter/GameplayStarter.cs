using System.Collections.Generic;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.ExternalServices;

namespace LD48Project.Starter {
    public class GameplayStarter : GameComponent {
	    [NotNull] public Submarine Submarine;

	    void Start() {
			#if UNITY_ANDROID
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