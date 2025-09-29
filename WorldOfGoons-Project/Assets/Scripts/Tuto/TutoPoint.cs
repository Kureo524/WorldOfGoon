using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TutoPoint : MonoBehaviour {
    public bool runtime = true;
    public Vector2 pointId;
    public List<TutoBar> connectedBars;

    private void Start() {
        if (runtime == false) {
            pointId = transform.position;
            if (TutoGameManager.AllPoints.ContainsKey(pointId) == false) {
                TutoGameManager.AllPoints.Add(pointId, this);
            }
        }
    }

    private void Update() {
        if (runtime == false) {
            if (transform.hasChanged == true) {
                transform.hasChanged = false;
                transform.position = Vector3Int.RoundToInt(transform.position);
            }
        }
    }
}
