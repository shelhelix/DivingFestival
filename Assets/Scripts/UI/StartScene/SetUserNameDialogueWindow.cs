using System;
using DG.Tweening;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using LD48Project.Leaderboards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD48Project.UI.StartScene {
	public class SetUserNameDialogueWindow : GameComponent {
		[NotNull] public Button         PlayAsAnonymous;
		[NotNull] public Button         SetUserName;
		
		[NotNull] public TMP_InputField UserNameInputField;

		Vector3 _startPosition;
		
		LeaderboardController _leaderboardController;

		Action _onHide;

		public void Init(LeaderboardController leaderboardController, Action onHide) {
			Hide(instant: true);
			_startPosition         = transform.position;
			_onHide                = onHide;
			_leaderboardController = leaderboardController;
			PlayAsAnonymous.onClick.AddListener(OnPlayAsAnonymous);
			SetUserName.onClick.AddListener(OnSetUserName);
		}

		public void Show() {
			transform.DOMove(Vector3.zero, 1).SetEase(Ease.OutExpo).SetDelay(0.1f);
			gameObject.SetActive(true);
		}

		public void Hide(bool instant = false) {
			if ( !instant ) {
				var seq = DOTween.Sequence();
				seq.Append(transform.DOMove(_startPosition, 1).SetEase(Ease.InExpo).SetDelay(0.1f));
				seq.AppendCallback(InternalHide);
			} else {
				InternalHide();
			}
		}

		void InternalHide() {
			_onHide?.Invoke();
			Deinit();
		}

		void Deinit() {
			_onHide = null;
			PlayAsAnonymous.onClick.RemoveAllListeners();
			SetUserName.onClick.RemoveAllListeners();
		}
		

		void OnPlayAsAnonymous() {
			_leaderboardController.SetUserName("Anonymous");
			Hide();
		}

		void OnSetUserName() {
			_leaderboardController.SetUserName(UserNameInputField.text);
			Hide();
		}
	}
}