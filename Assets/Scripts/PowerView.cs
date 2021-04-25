using UnityEngine.UI;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project {
	public class PowerView : GameComponent {
		[NotNull] public Submarine.Subsystem System;
		[NotNull] public TMP_Text  PowerUsageText;
		[NotNull] public TMP_Text  UpKey;
		[NotNull] public TMP_Text  DownKey;
		[NotNull] public Image     BackgroundImage;
	}
}