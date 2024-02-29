using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour {
    [SerializeField] Button button = null;
    [SerializeField] Text speedText = null;

	void Start() {
		
	}
	
	void Update() {
        button.interactable = !KenoGame.instance.isRevealingNumbers;
        speedText.text = "SPEED  " + KenoGame.instance.GetSpeedValueString();
	}

    public void OnButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);
        KenoGame.instance.OnSpeedButtonClicked();
    }
}
