using System.Collections.Generic;
using System.Text;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using GooglePlayGames.BasicApi;
using TMPro;

namespace LD48Project.UI {
	public class HighScoreUI : GameComponent {
		[NotNull] public TMP_Text Text;
		
		public void Init(LeaderboardScoreData scoreData, Dictionary<string, string> userIdToUserNameConverter) {
			var builder = new StringBuilder();
			if ( !scoreData.Valid ) {
				builder.Append("Can't load global highscore :(");
			}
			foreach ( var score in scoreData.Scores ) {
				builder.Append(score.rank)
					.Append(" ")
					.Append(userIdToUserNameConverter[score.userID])
					.Append(" ")
					.Append(score.formattedValue)
					.AppendLine();
			}
			Text.text = builder.ToString();
		}
	}
}