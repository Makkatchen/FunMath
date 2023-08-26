using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobInitMobileAds : MonoBehaviour
{
    [SerializeField] private AdMobBannerScript _bannerScript;
    [SerializeField] private AdMobInterstitialScript _interstitialScript;
    [SerializeField] private AdMobRewardScript _rewardedScript;
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            _bannerScript.CreateBannerView();
            _interstitialScript.LoadInterstitialAd();
            _rewardedScript.LoadRewardedAd();
        }
        );
    }

    public void ShowInterstitial()
    {
        _interstitialScript.ShowAd();
    }

    public void ShowRewarded()
    {
        _rewardedScript.ShowAdRewarded();
    }

}
