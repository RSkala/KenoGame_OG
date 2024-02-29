using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchVideoAdButton : MonoBehaviour {
    [SerializeField] Button button = null;

	void Start() {
		
	}
	
	void Update() {
        button.interactable = UnityAdsController.instance.IsVideoAdReady() && !KenoGame.instance.isRevealingNumbers;
	}

    public void OnButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);
        UnityAdsController.instance.ShowRewardedVideo();
    }
}
