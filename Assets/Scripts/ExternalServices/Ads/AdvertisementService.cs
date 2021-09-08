using System;
using LD48Project.Utils;
using UnityEngine.Advertisements;

namespace LD48Project.ExternalServices.Ads {
	public class AdvertisementService : Singleton<AdvertisementService> {
		AdvertisementWrapper _rewardAdWrapper;

		bool _isInited;
		
		public void Init() {
			if ( _isInited ) {
				return;
			}
			_isInited        = true;
			_rewardAdWrapper = new AdvertisementWrapper(AdvertisementConsts.RewardAdAndroid);
			Advertisement.Initialize(AdvertisementConsts.GameId);	
			Advertisement.AddListener(_rewardAdWrapper);
		}

		public void ShowAd(Action<bool> onAdShowed) {
			_rewardAdWrapper.ShowAd(onAdShowed);
		}
	}
}