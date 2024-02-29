using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCreditsText : MonoBehaviour {
    [SerializeField] Text userCreditsText = null;

    void Awake() {
        UpdateUserCreditsText();
    }
	
	void Update () {
        UpdateUserCreditsText();
	}

    void UpdateUserCreditsText() {
        userCreditsText.text = string.Format("{0:N0}", KenoGame.instance.playerCredits);
    }
}
