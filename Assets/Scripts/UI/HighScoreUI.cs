using System.Text;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using GooglePlayGames.BasicApi;
using TMPro;

namespace LD48Project.UI {
	public class HighScoreUI : GameComponent {
		[NotNull] public TMP_Text Text;
		
		public void Init(LeaderboardScoreData scoreData) {
			var builder = new StringBuilder();
			if ( !scoreData.Valid ) {
				builder.Append("Can't load global highscore :(");
			}
			foreach ( var score in scoreData.Scores ) {
				builder.Append(score.rank)
					.Append(" ")
					.Append(score.userID)
					.Append(" ")
					.Append(score.formattedValue)
					.AppendLine();
			}

			Text.text = builder.ToString();
		}
	}
}