using DG.Tweening;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;
using UnityEngine;

namespace LD48Project.UI {
	public class LeaderboardEntry : GameComponent {
		[NotNull] public TMP_Text Place;
		[NotNull] public TMP_Text UserName;
		[NotNull] public TMP_Text Depth;
		
		public void Init(int place, string userName, long depth) {
			Place.text    = place.ToString();
			UserName.text = userName;
			Depth.text    = depth.ToString();
			gameObject.SetActive(true);
		}

		public void Deinit() {
			gameObject.SetActive(false);
		}
	}
}