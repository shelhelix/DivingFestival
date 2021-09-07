using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace LD48Project.ExternalServices.Ads {
	public class AdvertisementWrapper : IUnityAdsListener {
		Action<bool> _onShowCompletedAction;

		readonly string _adId;
		
		public AdvertisementWrapper(string adId) {
			_adId = adId;
			LoadAd();
		}

		public void LoadAd() {
			Advertisement.Load(_adId);
		}

		public void ShowAd(Action<bool> onShowCompleted) {
			if ( !Advertisement.IsReady(_adId) ) {
				onShowCompleted?.Invoke(false);
				return;
			}
			_onShowCompletedAction = onShowCompleted;
			Advertisement.Show(_adId);
		}

		public void OnUnityAdsReady(string placementId) { }

		public void OnUnityAdsDidError(string message) {
			TryInvokeShowAction(false);
			Advertisement.Load(_adId);
		}

		public void OnUnityAdsDidStart(string placementId) {
			Debug.Log("show started");
		}

		public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
			Debug.Log("show ended");
			var isRewarded = showResult == ShowResult.Finished;
			TryInvokeShowAction(isRewarded);
			LoadAd();
		}

		void TryInvokeShowAction(bool result) {
			_onShowCompletedAction?.Invoke(result);
			_onShowCompletedAction = null;
		}
	}
}