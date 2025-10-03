using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour {
    public List<PointStateMachine> basePoints = new();
    private Dictionary<Vector2, Vector2> _pointsConnected = new();

    public void CheckForLinks() {
        // _pointsConnected.Clear();
        // foreach (PointStateMachine point in basePoints) {
        //     point.RemoveAllLinks();
        //     foreach (PointStateMachine secondPoint in basePoints) {
        //         if (point == secondPoint) continue;
        //         if (Vector2.SqrMagnitude((Vector2)secondPoint.transform.position - (Vector2)point.transform.position) <=
        //             point.dragRadius) {
        //             Link link = point.CreateLink(point.transform.position, secondPoint.transform.position);
        //             point.links.Add(link);
        //             secondPoint.links.Add(link);
        //         }
        //     }
        // }
    }

    private void Start() {
        foreach (PointStateMachine psm in basePoints) {
            Debug.Log("Initializing");
            psm.InitializeConnections();
        }
    }
    //
    // private IEnumerator SetDragPlace() {
    //     yield return new WaitForEndOfFrame();
    //     
    //     foreach(PointStateMachine point in basePoints) {
    //         point.UpdateState(PointStateMachine.PointStates.Dragged);
    //         yield return new WaitForEndOfFrame();
    //         yield return new WaitForEndOfFrame();
    //         yield return new WaitForEndOfFrame();
    //         point.UpdateState(PointStateMachine.PointStates.Placed);
    //     }
    // }
}
