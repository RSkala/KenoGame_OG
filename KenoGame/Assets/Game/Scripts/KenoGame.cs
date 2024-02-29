using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenoGame : MonoBehaviour {
    [SerializeField] KenoGrid kenoGridTop = null;
    [SerializeField] KenoGrid kenoGridBot = null;
    [SerializeField] GameObject kenoCellPrefab = null;
    [SerializeField] ClearGridButton clearGridButton = null;
    [SerializeField] StartButton startButton = null;
    [SerializeField] GameObject maxCellsFilledDialog = null;
    [SerializeField] GameObject outOfCreditsDialog = null;

    const int NUM_GRIDS = 2;
    const int NUM_KENO_CELLS_PER_GRID = 40;
    const int KENO_NUMBER_START_VALUE = 1;
    const int NUM_VALUES_TO_PICK = 20;
    public const int MAX_VALUES_TO_PICK = 10;
    public const int MIN_VALUES_PICKED_TO_PLAY = 2;
    const float PICK_REVEAL_TIME_SPEED_1 = 0.5f;
    const float PICK_REVEAL_TIME_SPEED_2 = 0.25f;
    const float PICK_REVEAL_TIME_SPEED_3 = 0.125f;
    const int REVEAL_SPEED_VALUE_DEFAULT = 1;

    const int USER_START_CREDITS = 10000;
    const int CREDITS_COST_PER_PICK = 50;
    const int CREDITS_AWARDED_FOR_WATCHING_VIDEO = 1000;
    const int CREDITS_AWARDED_FOR_CORRECT_PICK = 100; // TODO: Implement paytable

    // TODO: Implement and display stats for picked numbers (number of games played vs number of times numbers show up)

    /// The currently selected time between pick reveals (lower = slower)
    public int currentSpeedValue { get; private set; }

    public Action<List<int>> onGameStart;
    public Action onClearCells;
    public Action<List<int>> onNumbersPicked;
    public Action<int, List<int>> onPickedNumberReveal;

    /// List of numbers selected by the user
    List<int> selectedNumbers;

    /// Singleton Instance
    public static KenoGame instance { get; private set; }

    /// Number of values selected by the user
    public int NumSelectedNumbers { get { return selectedNumbers.Count; } }

    /// Whether the game is currently revealing the randomly picked numbers
    public bool isRevealingNumbers { get; private set; }

    // The player's game Credits
    public int playerCredits { get; private set; }

    void Awake() {
        instance = this;
        playerCredits = USER_START_CREDITS;
        currentSpeedValue = REVEAL_SPEED_VALUE_DEFAULT;
        InitKenoGrids();
        selectedNumbers = new List<int>();
    }

	void Start() {
        clearGridButton.Init(OnClearButtonPressed);
        startButton.Init(OnStartButtonPressed);
        startButton.DisableButton();
	}

    void InitKenoGrids() {
        kenoGridTop.Init(kenoCellPrefab, KENO_NUMBER_START_VALUE, NUM_KENO_CELLS_PER_GRID, OnKenoNumberSelected);
        kenoGridBot.Init(kenoCellPrefab, NUM_KENO_CELLS_PER_GRID + KENO_NUMBER_START_VALUE, NUM_KENO_CELLS_PER_GRID, OnKenoNumberSelected);
    }
	
	void Update() {
		
	}

    void OnKenoNumberSelected(int kenoNumber) {
        if(selectedNumbers.Contains(kenoNumber)) {
            selectedNumbers.Remove(kenoNumber);
        }
        else {
            selectedNumbers.Add(kenoNumber);
        }
    }

    void OnClearButtonPressed() {
        selectedNumbers.Clear();
        startButton.DisableButton();
        onClearCells();
    }

    void OnStartButtonPressed() {
        if(!CanAffordToPlay()) {
            outOfCreditsDialog.SetActive(true);
            return;
        }
        SubtractCredits(selectedNumbers.Count * CREDITS_COST_PER_PICK);
        onGameStart(selectedNumbers);
        HandleStartPicks();
    }

    void HandleStartPicks() {
        isRevealingNumbers = true;
        List<int> pickedValues = RandomlyPickNumbers();
        StartPickReveals(pickedValues);
    }

    List<int> RandomlyPickNumbers() {
        // Create the list that will be filled with "picked" values
        List<int> pickedValues = new List<int>();

        // Get the list of all the possible values
        List<int> possibleValues = GeneratePossibleValueList();

        // Pick specified number of values from the list of possible values
        for(int i = 0; i < MAX_VALUES_TO_PICK; ++i) {
            // Get a random index into the list of possible values
            int randomIndex = UnityEngine.Random.Range(0, possibleValues.Count);

            // Add the value to the list of picked values
            pickedValues.Add(possibleValues[randomIndex]);

            // Remove the value from the possible value list
            possibleValues.RemoveAt(randomIndex);
        }

        DEBUG_PrintPickedValues(pickedValues);

        // Send the pickedValues list to all listeners
        //onNumbersPicked(pickedValues);

        return pickedValues;
    }

    List<int> GeneratePossibleValueList() {
        List<int> valueList = new List<int>();
        for(int i = 1; i <= NUM_KENO_CELLS_PER_GRID * NUM_GRIDS; ++i) {
            valueList.Add(i);
        }
        return valueList;
    }

    void StartPickReveals(List<int> pickedValues) {
        RevealPickValueFromList(pickedValues, 0.0f);
    }

    void RevealPickValueFromList(List<int> pickedValues, float delay) {
        int newPickedValue = pickedValues[0];
        pickedValues.RemoveAt(0);

        StartCoroutine(RevealPick(newPickedValue, pickedValues, delay));
    }

    IEnumerator RevealPick(int pickedValue, List<int> pickedValues, float delay) {
        yield return new WaitForSeconds(delay);

        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.PopSound);
        onPickedNumberReveal(pickedValue, selectedNumbers);

        // If this revealed pick is one of the user's marked cell numbers
        if(selectedNumbers.Contains(pickedValue)) {
            // Play the "win" sound
            AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.CoinRing);

            // Award credits for a correct pick
            AwardCredits(CREDITS_AWARDED_FOR_CORRECT_PICK);
        }

        if(pickedValues.Count > 0) {
            RevealPickValueFromList(pickedValues, GetTimeBetweenPickReveals());
        }
        else {
            isRevealingNumbers = false;
        }
    }

    public void ShowMaxCellsFilledDialog() {
        maxCellsFilledDialog.SetActive(true);
    }

    // Return the time between pick reveals, depending on the current speed value (lower = slower)
    float GetTimeBetweenPickReveals() {
        switch(currentSpeedValue) {
            case 1: return PICK_REVEAL_TIME_SPEED_1;
            case 2: return PICK_REVEAL_TIME_SPEED_2;
            case 3: return PICK_REVEAL_TIME_SPEED_3;
            default: return PICK_REVEAL_TIME_SPEED_2;
        }
    }

    public string GetSpeedValueString() {
        switch(currentSpeedValue) {
            case 1: return ">";
            case 2: return ">>";
            case 3: return ">>>";
            default: return ">>";
        }
    }

    public void OnSpeedButtonClicked() {
        currentSpeedValue++;
        if(currentSpeedValue > 3) {
            currentSpeedValue = 1;
        }
    }

    void DEBUG_PrintPickedValues(List<int> pickedValues) {
        string pickedValuesString = "Picked Values:  ";
        foreach(int pickedValue in pickedValues)
        {
            pickedValuesString += pickedValue.ToString() + "  ";
        }
        Debug.Log(pickedValuesString + "\n");
    }

    void AwardCredits(int creditsToAward) {
        playerCredits += creditsToAward;
    }

    void SubtractCredits(int creditsToSubtract) {
        playerCredits -= creditsToSubtract;
        playerCredits = Mathf.Max(0, playerCredits);
    }

    bool CanAffordToPlay() {
        return playerCredits >= selectedNumbers.Count * CREDITS_COST_PER_PICK;
    }

    public void OnRewardedVideoWatched() {
        // Play the video reward sound
        AudioPlayback.instance.PlaySoundEffect(AudioPlayback.SFX.CashRegister);

        // Award credits for watching a video
        AwardCredits(CREDITS_AWARDED_FOR_WATCHING_VIDEO);
    }
}
