using System;
using UnityEngine;
//using GoogleMobileAds.Api;


public class AdMobInterstitialScript : MonoBehaviour
{
    /*
    private const string _adUnitIdInterstitia = "ca-app-pub-3940256099942544/1033173712";
        //"ca-app-pub-2462155519154813/7838601841"; //-work //"ca-app-pub-3940256099942544/1033173712" - test;

    private InterstitialAd _interstitialAd;

    public bool _isLoadInterstitial;

    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            DestroyAdInterstitial();
        }

        Debug.Log("Loading the interstitial ad.");

        _interstitialAd = new InterstitialAd(_adUnitIdInterstitia);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(adRequest);

        _interstitialAd.OnAdLoaded += LoadAdInterstitial;
        _interstitialAd.OnAdFailedToLoad += FailLoadAdInterstitial;
    }

    public void ShowAd()
    {
        if (_interstitialAd.IsLoaded())
            _interstitialAd.Show();
        else
            Debug.LogWarning("Interstitial is not loaded");
    }

    public void DestroyAdInterstitial()
    {
        if (_interstitialAd != null)
        {
            Debug.Log("Destroying interstitial ad.");
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
    }

    private void LoadAdInterstitial(object sender, EventArgs args)
    {
        _isLoadInterstitial = true;
        Debug.Log("LoadAdInterstitial" + _isLoadInterstitial);
    }
    private void FailLoadAdInterstitial(object sender, EventArgs args)
    {
        _isLoadInterstitial = false;
        Debug.Log("LoadAdInterstitial" + _isLoadInterstitial);
    }

    private void ShowCompleteAdInterstitial(object sender, EventArgs args)
    {
        _isLoadInterstitial = false;
        Debug.Log("LoadAdInterstitial" + _isLoadInterstitial);
    }

    private void OnDisable()
    {
        _interstitialAd.OnAdLoaded -= LoadAdInterstitial;
        _interstitialAd.OnAdFailedToLoad -= FailLoadAdInterstitial;
    }
    */
}
