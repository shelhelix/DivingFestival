using DG.Tweening;
using GameComponentAttributes.Attributes;
using ThisOtherThing.UI.Shapes;

namespace LD48Project.UI.PowerPanel {
	public class ArcPowerBar : BasePowerBar {
		[NotNull] public Arc Progress;

		public int MaxAmount;

		Tween _activeTween;
		
		public override void SetValue(int value) {
			var progress = (value == MaxAmount) ? 1f : (float)value / MaxAmount;
			_activeTween?.Kill();
			_activeTween = DOTween.To(() => Progress.ArcProperties.Length, (x) => {
				Progress.ArcProperties.Length = x;
				Progress.ForceMeshUpdate();
			}, progress, 0.2f).Play();
		} 
	}
}