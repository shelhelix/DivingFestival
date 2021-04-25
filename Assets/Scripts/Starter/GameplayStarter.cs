using System.Collections.Generic;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;

namespace LD48Project.Starter {
    public class GameplayStarter : GameComponent {
	    [NotNull] public Submarine Submarine;

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