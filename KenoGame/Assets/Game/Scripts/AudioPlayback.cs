using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayback : MonoBehaviour {
    [Header("SFX")]
    [SerializeField] AudioSource buttonClick = null;
    [SerializeField] AudioSource popSound = null;
    [SerializeField] AudioSource coinRing = null;
    [SerializeField] AudioSource cashRegister = null;

    public static AudioPlayback instance { get; private set; }

    public enum SFX {
        ButtonClick,
        PopSound,
        CoinRing,
        CashRegister
    }

    void Awake() {
        instance = this;
    }

    void Start() {
		
	}

    public void PlaySoundEffect() {
        
    }

    public void PlaySoundEffect(SFX sfx) {
        switch(sfx) {
            case SFX.ButtonClick: buttonClick.Play(); break;
            case SFX.PopSound: popSound.Play(); break;
            case SFX.CoinRing: coinRing.Play(); break;
            case SFX.CashRegister: cashRegister.Play(); break;
        }
    }
}
