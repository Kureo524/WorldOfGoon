using System.Collections.Generic;
using UnityEngine;

public class TutoGameManager : MonoBehaviour {
    public static Dictionary<Vector2, TutoPoint> AllPoints = new Dictionary<Vector2, TutoPoint>();

    private void Awake() {
        AllPoints.Clear();
    }
}
