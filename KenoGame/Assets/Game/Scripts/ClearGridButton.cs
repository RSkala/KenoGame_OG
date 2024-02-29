using System;
using UnityEngine;
using UnityEngine.UI;

public class ClearGridButton : MonoBehaviour {
    [SerializeField] Button button = null;

    Action onClearButtonPressed;

	void Start() {
		
	}

    public void Init(Action onClearButtonPressed) {
        this.onClearButtonPressed = onClearButtonPressed;
    }

    void Update() {
        button.interactable = !KenoGame.instance.isRevealingNumbers;
    }

    public void OnButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);
        onClearButtonPressed();
    }
}
