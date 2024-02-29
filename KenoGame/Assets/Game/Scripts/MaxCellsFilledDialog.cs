using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxCellsFilledDialog : MonoBehaviour {

	void Start() {
		
	}
	
	void Update() {
		
	}

    public void OnMaxCellsFilledDialogCloseButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);

        // Hide this view
        gameObject.SetActive(false);
    }
}
