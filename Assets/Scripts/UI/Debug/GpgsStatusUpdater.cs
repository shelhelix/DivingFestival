using System;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.ExternalServices;
using TMPro;

namespace LD48Project.UI.Debug {
	public class GpgsStatusUpdater : GameComponent {
		[NotNull] public TMP_Text Text;

		public void Update() {
			Text.text =
				$"Is GPGS Logged in: {GooglePlayGamesService.Instance.IsLoggedIn}. Reason: {GooglePlayGamesService.Instance.LoginFailReason}";
		}
	}
}