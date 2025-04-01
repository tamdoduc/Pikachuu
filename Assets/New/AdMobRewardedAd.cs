using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;

public class AdMobRewardedAd : MonoBehaviour
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-1150048591169357/8984919058";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
     private string _adUnitId = "ca-app-pub-1150048591169357/8992765711";
#endif

    private RewardedAd _rewardedAd;

    // Singleton instance
    private static AdMobRewardedAd _instance;

    // Lock object for thread safety
    private static readonly object _lock = new object();

    // Public property to access the singleton instance
    public static AdMobRewardedAd Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    // Find the instance in the scene or create a new one if it doesn't exist
                    _instance = FindObjectOfType<AdMobRewardedAd>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<AdMobRewardedAd>();
                        singletonObject.name = typeof(AdMobRewardedAd).ToString() + " (Singleton)";
                    }
                }
                return _instance;
            }
        }
    }

    // Ensure the instance is destroyed when the application quits
    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private void Start()
    {
        LoadRewardedAd(null);
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd(UnityAction unityActionOnComplete)
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // Tạo request để load quảng cáo
        var adRequest = new AdRequest();

        // Gửi yêu cầu tải quảng cáo
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded successfully.");
                _rewardedAd = ad;
                unityActionOnComplete?.Invoke();
            });
    }


    /// <summary>
    /// Shows the rewarded ad.
    /// </summary>
    public void ShowRewardedAd(UnityAction onRewardEarned)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Reward earned: " + reward.Amount);
                onRewardEarned?.Invoke();
                LoadRewardedAd(null);
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
            LoadRewardedAd(null);
        }
    }
}
