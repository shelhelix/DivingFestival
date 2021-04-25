using System.Collections.Generic;

using GameComponentAttributes;

namespace LD48Project.Starter {
    public class GameplayStarter : GameComponent {
	    void Start() {
		    InitComponents();
	    }

	    void InitComponents() {
		    var comps = new List<GameplayComponent>(GameplayComponent.Instances);
		    foreach ( var comp in comps ) {
			    comp.Init(this);
		    }
	    }
    }
}