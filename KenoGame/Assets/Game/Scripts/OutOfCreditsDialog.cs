using UnityEngine;
using UnityEngine.UI;

public class OutOfCreditsDialog : MonoBehaviour {
    [SerializeField] Button okButton = null;

	void Start() {
		
	}
	
	void Update() {
        okButton.interactable = UnityAdsController.instance.IsVideoAdReady();
	}

    public void OnButtonClickedCancel() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);

        // Cancelled: Just close this dialog
        gameObject.SetActive(false);
    }

    public void OnButtonClickedOK() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);

        // Show the rewarded video then close this dialog
        UnityAdsController.instance.ShowRewardedVideo();
        gameObject.SetActive(false);
    }
}
