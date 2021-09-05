using System;
using LD48Project.Utils;
using UnityEngine;
using UnityEngine.Advertisements;

namespace LD48Project.ExternalServices.Ads {
	public class AdvertisementService : Singleton<AdvertisementService>, IUnityAdsInitializationListener {
		AdvertisementWrapper _rewardAdWrapper;
		public void Init() {
			Advertisement.Initialize(AdvertisementConsts.GameId, false, false, this);	
		}

		public void LoadAd() {
			_rewardAdWrapper.LoadAd();
		}

		public void ShowAd(Action<bool> onAdShowed) {
			_rewardAdWrapper.ShowAd(onAdShowed);
		}

		public void OnInitializationComplete() {
			_rewardAdWrapper = new AdvertisementWrapper(AdvertisementConsts.RewardAdAndroid);
		}

		public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
			Debug.Log($"Failed to initialize unity ads. Reason: {error} - {message}");
		}
	}
}