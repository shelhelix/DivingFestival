using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;
using UnityEngine;

public class VersionUpdater : GameComponent {
	[NotNull] public TMP_Text Text;
    // Update is called once per frame
    void Update() {
		Text.text = $"{Application.version}";
	}
}
