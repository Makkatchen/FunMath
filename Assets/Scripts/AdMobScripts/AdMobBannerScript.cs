using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobBannerScript : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-2462155519154813/9387471323";// - work //"ca-app-pub-3940256099942544/6300978111"; - test

    private BannerView _bannerView;

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAdBanner();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        LoadAd();
        _bannerView.OnAdLoaded += LoadAdBanner;
    }

    private void LoadAd()
    {
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        AdRequest request = new AdRequest.Builder().Build();



        _bannerView.LoadAd(request);

        Debug.Log("LoadAd");
    }

    private void LoadAdBanner(object sender, EventArgs args)
    {
        Debug.Log("LoadAdBanner banner.");
    }

    private void DestroyAdBanner()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    private void OnDisable()
    {
       // _bannerView.OnAdLoaded -= LoadAdBanner;
    }
}
