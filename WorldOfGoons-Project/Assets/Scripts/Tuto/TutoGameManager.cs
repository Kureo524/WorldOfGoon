using System.Collections.Generic;
using UnityEngine;

public class TutoGameManager : MonoBehaviour {
    public static Dictionary<Vector2, Point> AllPoints = new Dictionary<Vector2, Point>();

    private void Awake() {
        AllPoints.Clear();
    }
}
