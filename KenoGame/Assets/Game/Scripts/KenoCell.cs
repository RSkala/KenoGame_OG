using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KenoCell : MonoBehaviour {
    [SerializeField] Button button = null;
    [SerializeField] Text kenoNumberText = null;
    [SerializeField] Image bgImage = null;
    [SerializeField] Color unselectedColor = Color.blue;
    [SerializeField] Color selectedColor = Color.red;
    [SerializeField] Color pickedColor = Color.yellow;
    [SerializeField] Color winnerColor = Color.green;

    int kenoNumber;
    Action<int> onKenoNumberSelected;
    bool isSelected;

    void Awake() {
        isSelected = false;
    }

	void Start() {
		
	}

    public void Init(int kenoNumber, Action<int> onKenoNumberSelected) {
        // Set the GameObject active in case the prefab was saved as inactive
        gameObject.SetActive(true);

        this.kenoNumber = kenoNumber;
        this.onKenoNumberSelected = onKenoNumberSelected;

        kenoNumberText.text = this.kenoNumber.ToString();
        gameObject.name = "KenoCell " + this.kenoNumber;

        KenoGame.instance.onGameStart += OnGameStart;
        KenoGame.instance.onClearCells += OnClearCells;
        KenoGame.instance.onNumbersPicked += OnNumbersPicked;
        KenoGame.instance.onPickedNumberReveal += OnPickedNumberReveal;
    }
	
	void Update() {
        // Disable input if the game is currently revealing numbers
        button.enabled = !KenoGame.instance.isRevealingNumbers;
	}

    public void OnButtonClicked() {
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.ButtonClick);

        // Show the "Max Cells Marked" dialog if the user has filled the max number of cells and is trying to mark an unmarked cell
        if(KenoGame.instance.NumSelectedNumbers >= KenoGame.MAX_VALUES_TO_PICK && !isSelected) {
            KenoGame.instance.ShowMaxCellsFilledDialog();
            return;
        }

        isSelected = !isSelected;
        bgImage.color = isSelected ? selectedColor : unselectedColor;

        onKenoNumberSelected(kenoNumber);
    }

    void OnClearCells() {
        isSelected = false;
        bgImage.color = unselectedColor;
    }

    void OnGameStart(List<int> selectedNumbers) {
        if(!selectedNumbers.Contains(kenoNumber)) {
            // Set the color to "unpicked" if this Keno Number was not in the selected numbers list
            bgImage.color = unselectedColor;
        }
        else {
            // This Keno Number was picked. Set to "picked" color
            bgImage.color = selectedColor;
        }
    }

    void OnNumbersPicked(List<int> pickedNumbers) {
        if(pickedNumbers.Contains(kenoNumber)) {
            bgImage.color = pickedColor;
        }
    }

    void OnPickedNumberReveal(int pickedNumber, List<int> pickedNumbers) {
        if(pickedNumber == kenoNumber) {
            if(pickedNumbers.Contains(kenoNumber)) {
                // This cell contains a picked number that was also marked by the user. Set to "Winner" color.
                bgImage.color = winnerColor;
            }
            else {
                // This cell contains a picked number, but was not marked by the user. Set to "Picked" color.
                bgImage.color = pickedColor;   
            }
        }
    }
}
