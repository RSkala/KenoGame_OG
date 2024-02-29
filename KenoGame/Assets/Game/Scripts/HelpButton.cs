using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour {
    [SerializeField] Button button = null;
    [SerializeField] HelpView helpView = null;

	void Start() {
		
	}

    void Update() {
        button.interactable = !KenoGame.instance.isRevealingNumbers;
    }

    public void OnButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);
        helpView.gameObject.SetActive(true);
    }
}
