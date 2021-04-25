using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;
using UnityEngine;

namespace LD48Project.UI {
	public class EndgameWindow : GameComponent {
		const string DepthDescTemplate = "Your max depth is: \n{0:.0}";
		
		[NotNull] public TMP_Text DepthDescText;
		[NotNull] public Button ReturnToMenuButton;
		[NotNull] public CanvasGroup GameplayUI;

		[NotNull] public Transform CenterPoint;
		
		public void Init(float depth) {
			DepthDescText.text = string.Format(DepthDescTemplate, depth);
			ReturnToMenuButton.onClick.AddListener(() => SceneManager.LoadScene("StartMenu"));
			GameplayUI.DOFade(0f, 0.5f);
			transform.DOMove(CenterPoint.position, 1f);
		}

		void OnDestroy() {
			ReturnToMenuButton.onClick.RemoveAllListeners();
		}
	}
}