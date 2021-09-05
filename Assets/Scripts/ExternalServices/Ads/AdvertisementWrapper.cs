using System;
using UnityEngine.Advertisements;

namespace LD48Project.ExternalServices.Ads {
	public class AdvertisementWrapper : IUnityAdsLoadListener, IUnityAdsShowListener {
		Action<bool> _onShowCompletedAction;

		readonly string _adId;
		
		public AdvertisementWrapper(string adId) {
			_adId = adId;
			LoadAd();
		}

		public void LoadAd() {
			Advertisement.Load(_adId, this);
		}

		public void ShowAd(Action<bool> onShowCompleted) {
			if ( !Advertisement.IsReady(_adId) ) {
				onShowCompleted?.Invoke(false);
				return;
			}
			_onShowCompletedAction = onShowCompleted;
			Advertisement.Show(_adId, this);
		}

		public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
			Advertisement.Load(_adId, this);
		}

		public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
			TryInvokeShowAction(false);
			LoadAd();
		}

		public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
			var isRewarded = showCompletionState == UnityAdsShowCompletionState.COMPLETED;
			TryInvokeShowAction(isRewarded);
			LoadAd();
		}
		
		public void OnUnityAdsAdLoaded(string placementId) { }

		public void OnUnityAdsShowStart(string placementId) { }

		public void OnUnityAdsShowClick(string placementId) { }

		void TryInvokeShowAction(bool result) {
			_onShowCompletedAction?.Invoke(result);
			_onShowCompletedAction = null;
		} 
	}
}