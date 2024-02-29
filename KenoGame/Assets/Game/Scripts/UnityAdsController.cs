using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsController : MonoBehaviour {
    // Get your Game ID to be used in your code:
    // Apple App Store: 1729909
    // Google Play Store: 1729908

    // Get these values from Unity Dashboard -> Monetization -> Platforms
    const string AppStoreId_Apple = "2891825";
    const string AppStoreId_Google = "2891827";

    public static UnityAdsController instance { get; private set; }

    //------------------------------------------------------------

#if !UNITY_IOS && !UNITY_ANDROID
    string GetUnityGameId() {
        Debug.LogWarning("Invalid Game Id");
        return "INVALID GAME ID";
    }
#endif

#if UNITY_IOS
    string GetUnityGameId() {
        return AppStoreId_Apple;
    }

#endif // UNITY_IOS

    //------------------------------------------------------------

#if UNITY_ANDROID
    string GetUnityGameId() {
        return AppStoreId_Google;
    }

#endif // UNITY_ANDROID

    //------------------------------------------------------------

    void Awake() {
        instance = this;
        Advertisement.debugMode = true;
        Advertisement.Initialize(GetUnityGameId());
    }

    void Start() {
		
	}
	
	void Update() {
        
	}

    public bool IsVideoAdReady() {
        return Advertisement.IsReady(); // TODO: USE PLACEMENT ID HERE
    }

    public void ShowRewardedVideo() {
        Debug.Log("UnityAdsController.ShowRewardedVideo\n");
        //Advertisement.Show();

        ShowOptions showOptions = new ShowOptions();
        showOptions.resultCallback = HandleShowResult;

        Advertisement.Show(showOptions); // TODO: USE PLACEMENT ID HERE
    }

    void HandleShowResult(ShowResult showResult) {
        Debug.Log("UnityAdsController.HandleShowResult - showResult: " + showResult + "\n");
        switch(showResult)
        {
            case ShowResult.Failed:
                // Do nothing
                break;

            case ShowResult.Skipped:
                // Do Nothing
                break;

            case ShowResult.Finished:
                KenoGame.instance.OnRewardedVideoWatched();
                break;
        }
    }
}
