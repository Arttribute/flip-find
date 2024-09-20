using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleAdsInitializer : MonoBehaviour
{
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        //CreateBannerView();
    }


#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-8809110862008740/7892515696";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adUnitId = "unused";
#endif

    BannerView _bannerView;


    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // // If we already have a banner, destroy the old one.
        // if (_bannerView != null)
        // {
        //     DestroyAd();
        // }

        // Create a 320x50 banner views at coordinate (0,50) on screen.
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }
}