using System.Collections.Generic;
using DG.Tweening;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.Leaderboards;
using TMPro;
using UnityEngine;

namespace LD48Project.UI {
	public class LeaderBoard : GameComponent {
		[NotNullOrEmpty] public List<LeaderboardEntry> Entries;
		[NotNull]        public TMP_Text               LoadingText;

		public bool Inited { get; private set;  }
		
		public async void Init(LeaderboardController leaderboardController) {
			foreach ( var entry in Entries ) {
				entry.Deinit();
			}
			LoadingText.gameObject.SetActive(true);
			LoadingText.transform.localScale = Vector3.one;
			LoadingText.text           = "Loading leaderboard...";
			var scores = await leaderboardController.GetScoresAroundPlayerAsync(Entries.Count);
			if ( scores == null ) {
				LoadingText.text = "Can't load global leaderboard";
				return;
			}
			var entryIndex = 0;
			foreach ( var score in scores ) {
				Entries[entryIndex].Init(score.Rank, score.UserName, score.ScoreValue);
				entryIndex++;
			}

			Inited = true;
		}

		public void PrepareForAnimation() {
			foreach ( var entry in Entries ) {
				if ( !entry.gameObject.activeSelf ) {
					continue;
				}
				entry.transform.localScale = Vector3.zero;
			}
		}

		public Sequence GetShowSeq() {
			var seq = DOTween.Sequence();
			seq.Append(LoadingText.transform.DOScale(Vector3.zero, 0.3f));
			foreach ( var entry in Entries ) {
				if ( !entry.gameObject.activeSelf ) {
					continue;
				}
				seq.Append(entry.transform.DOScale(Vector3.one, 0.5f));
			}
			seq.Pause();
			return seq;
		}
	}
}