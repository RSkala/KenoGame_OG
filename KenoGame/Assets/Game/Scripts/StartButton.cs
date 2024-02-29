using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
    [SerializeField] Button button = null;

    Action onStartButtonPressed;

    void Start() {

    }

    public void Init(Action onStartButtonPressed) {
        this.onStartButtonPressed = onStartButtonPressed;
    }

    void Update() {
        button.interactable = !KenoGame.instance.isRevealingNumbers && KenoGame.instance.NumSelectedNumbers >= KenoGame.MIN_VALUES_PICKED_TO_PLAY;
    }

    public void OnButtonClicked(){
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);
        onStartButtonPressed();
    }

    public void DisableButton() {
        button.interactable = false;
    }

    public void EnableButton() {
        button.interactable = true;
    }
}
