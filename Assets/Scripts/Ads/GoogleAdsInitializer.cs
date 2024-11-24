using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleAdsInitializer : MonoBehaviour
{
    public static GoogleAdsInitializer Instance;

    private BannerView _bannerView;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-8809110862008740/7892515696";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adUnitId = "unused";
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
        });

    }


    public void RequestBanner()
    {
        if (!IsOnline())
        {
            Debug.LogWarning("Cannot request banner ad. No internet connection.");
            return;
        }

        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a banner view at bottom of the screen.
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }

    }

    public bool IsOnline()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}