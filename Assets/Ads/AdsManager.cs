//using ByteDance.Union;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : Singleton<AdsManager>, IUnityAdsLoadListener,IUnityAdsInitializationListener
{
    [SerializeField] string _androidAdGameId = "Interstitial_Android";
    [SerializeField] string _iOsAdGameId = "Interstitial_iOS";
    string _gameId;

    [SerializeField] string _iOsAdUnitId = "Rewarded_iOS";
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    public string _unitId;


    [SerializeField] string _iosInterstitial = "Interstitial_iOS";
    [SerializeField] string _androidInterstitial = "Interstitial_Android";
    public string _interstitialId;


    public bool testMode = true;

    public static bool isChina = false;

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Loaded Ad: " + _unitId);
    }

    private void Awake()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdGameId
            : _androidAdGameId;
        _unitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
        _interstitialId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iosInterstitial
            : _androidInterstitial;
        //Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, testMode,this);
        Debug.Log("unity load ads "+ Advertisement.isInitialized);
        Advertisement.Load(_unitId, this);
        Advertisement.Load(_interstitialId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Unity Ads Load Failed: {error.ToString()} - {message}");
    }

    public void Load()
    {
        if (isChina)
        {
           // PangleAdsManager.Instance.LoadRewardAd();
        }
        else
        {
            Advertisement.Load(_unitId, this);
        }
    }

    //public void LoadInter()
    //{

    //    Advertisement.Load(_interstitialId, this);
    //}

    //public void showInterAd(IUnityAdsShowListener listener)
    //{
    //    if (!Advertisement.isInitialized)
    //    {
    //        Advertisement.Initialize(_gameId, testMode);  //// 1st parameter is String and 2nd is boolean
    //    }
    //    if (Advertisement.isInitialized)
    //    {

    //        Debug.Log("Showing Ad: " + _interstitialId);
    //        Advertisement.Show(_interstitialId, listener);
    //    }
    //    else
    //    {
    //        Debug.LogError("ad no initialized");
    //    }
    //    // Load another ad:
    //    AdsManager.Instance.LoadInter();
    //}

    public void ShowAd(GameObject listener)
    {
        if (isChina)
        {
            Debug.Log("is in china");
            //PangleAdsManager.Instance.ShowRewardAd(listener);
        }
        else
        {
            Debug.Log("is not in china");
            if (listener.GetComponent<IUnityAdsShowListener>()!=null)
            {
                ShowAd(listener.GetComponent<IUnityAdsShowListener>());

            }
            else
            {
                Debug.LogError("listener does not support ads in china " + isChina);
            }
        }
    }

    public void showUnityAd(GameObject listener)
    {

        if (listener.GetComponent<IUnityAdsShowListener>() != null)
        {
            ShowAd(listener.GetComponent<IUnityAdsShowListener>());

        }
        else
        {
            Debug.LogError("listener does not support ads in china " + isChina);
        }
    }


    private void ShowAd(IUnityAdsShowListener listener)
    {

        Debug.Log("will show ad in unity");
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(_gameId, testMode);  //// 1st parameter is String and 2nd is boolean
        }
        if (Advertisement.isInitialized)
        {

            Debug.Log("Showing Ad: " + _unitId);
            Advertisement.Show(_unitId, listener);
        }
        else
        {
            Debug.LogError("ad no initialized");
        }
        // Load another ad:
        //AdsManager.Instance.Load();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
