using UnityEngine.UI;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project {
	public class BasicItemView : GameComponent {
		[NotNull] public TMP_Text Text;
		[NotNull] public Image    Background;
	}
}