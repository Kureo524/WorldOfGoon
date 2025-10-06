using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonLinker : MonoBehaviour {
    public List<PointStateMachine> basePoints = new();
    
    private void Start() {
        foreach (PointStateMachine psm in basePoints) {
            Debug.Log("Initializing");
            psm.InitializeConnections();
        }
    }
}
