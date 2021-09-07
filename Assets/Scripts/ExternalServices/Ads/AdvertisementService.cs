using System;
using LD48Project.Utils;
using UnityEngine.Advertisements;

namespace LD48Project.ExternalServices.Ads {
	public class AdvertisementService : Singleton<AdvertisementService> {
		AdvertisementWrapper _rewardAdWrapper;
		public void Init() {
			_rewardAdWrapper = new AdvertisementWrapper(AdvertisementConsts.RewardAdAndroid);
			Advertisement.Initialize(AdvertisementConsts.GameId);	
			Advertisement.AddListener(_rewardAdWrapper);
		}

		public void LoadAd() {
			_rewardAdWrapper.LoadAd();
		}

		public void ShowAd(Action<bool> onAdShowed) {
			_rewardAdWrapper.ShowAd(onAdShowed);
		}
	}
}