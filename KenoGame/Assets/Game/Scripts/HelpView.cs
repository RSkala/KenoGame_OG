using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpView : MonoBehaviour {

	void Start() {
		
	}
	
	void Update() {
		
	}

    public void OnHelpViewCloseButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);

        // Hide this view
        gameObject.SetActive(false);
    }
}
