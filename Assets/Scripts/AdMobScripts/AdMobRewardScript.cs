using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobRewardScript : MonoBehaviour
{
    private string _adUnitIdRewarder = "ca-app-pub-2462155519154813/9525705147"; //-work "ca-app-pub-3940256099942544/5224354917"; - test

    public bool _isLoadRewarded;


    private RewardedAd _rewardedAd;

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            DestroyAdRewarded();
        }

        Debug.Log("Loading the rewarded ad.");

        _rewardedAd = new RewardedAd(_adUnitIdRewarder);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(adRequest);

        _rewardedAd.OnAdLoaded += LoadAdRewarded;
        _rewardedAd.OnAdFailedToLoad += FailLoadAdRewarded;
        _rewardedAd.OnUserEarnedReward += AdRewardedIsComplete;

    }

    public void DestroyAdRewarded()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying interstitial ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }

    public void ShowAdRewarded()
    {
        if (_rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial is not loaded");
        }
    }


    private void LoadAdRewarded(object sender, EventArgs args)
    {
        _isLoadRewarded = true;
    }
    private void FailLoadAdRewarded(object sender, EventArgs args)
    {
        _isLoadRewarded = false;
    }
    private void AdRewardedIsComplete(object sender, EventArgs args)
    {
        AddReward();
    }

    private void AddReward()
    {
        NavigationGame.instance.ShowAnswerForReward();
        _rewardedAd.OnAdLoaded -= LoadAdRewarded;
        _rewardedAd.OnAdFailedToLoad -= FailLoadAdRewarded;
        _rewardedAd.OnUserEarnedReward -= AdRewardedIsComplete;
        DestroyAdRewarded();
        LoadRewardedAd();
    }

    private void OnDisable()
    {
        //_rewardedAd.OnAdLoaded -= LoadAdRewarded;
        //_rewardedAd.OnAdFailedToLoad -= FailLoadAdRewarded;
        //_rewardedAd.OnUserEarnedReward -= AdRewardedIsComplete;
    }
}
