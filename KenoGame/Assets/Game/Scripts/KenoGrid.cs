using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenoGrid : MonoBehaviour {
    List<KenoCell> kenoCells;

    void Awake() {
        
    }

	void Start() {
		
	}

    public void Init(GameObject kenoCellPrefab, int kenoNumberStartVal, int maxKenoGridCells, Action<int> onKenoNumberSelected) {
        InitKenoCells(kenoCellPrefab, kenoNumberStartVal, maxKenoGridCells, onKenoNumberSelected);
    }

    void InitKenoCells(GameObject kenoCellPrefab, int kenoNumberStartVal, int maxKenoGridCells, Action<int> onKenoNumberSelected) {
        kenoCells = new List<KenoCell>();
        for(int i = 0; i < maxKenoGridCells; ++i) {
            KenoCell kenoCell = Instantiate(kenoCellPrefab, transform).GetComponent<KenoCell>();
            kenoCell.Init(i + kenoNumberStartVal, onKenoNumberSelected);
            kenoCells.Add(kenoCell);
        }
    }
	
	void Update() {
		
	}
}
