using UnityEngine;
using UnityEngine.UI;

using LD48Project.Starter;

using DG.Tweening;
using GameComponentAttributes.Attributes;
using TMPro;

namespace LD48Project.UI {
	public class HpMeter : GameplayComponent {
		[NotNull] public TMP_Text Text;
		[NotNull] public Image    Background;

		int _value;
		
		public override void Init(GameplayStarter starter) {
			starter.Submarine.Hp.OnValueChanged += UpdateText;
			UpdateText(starter.Submarine.Hp.Value);
		}

		void UpdateText(int value) {
			var valueToShow = Mathf.Max(value, 0);
			Text.text = valueToShow.ToString();
			if ( value < _value ) {
				RunRedAnimation(Background);
			}
			_value = value;
		}
		
		void RunRedAnimation(Image image) {
			var seq = DOTween.Sequence();
			seq.Append(image.DOColor(Color.red, 0.3f));
			seq.Append(image.DOColor(Color.white, 0.3f));
		}
	}
}