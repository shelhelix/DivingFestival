using LD48Project.Starter;

using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI.PowerPanel {
	public abstract class BasePowerItem : GameplayComponent {
		[NotNull] public PowerBar Bar;
	}
}