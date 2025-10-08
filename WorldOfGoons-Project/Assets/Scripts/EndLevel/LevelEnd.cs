using System;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    public float checkRadius;
    public string goonLayer;
    
    public void Update() {
        Collider2D goonCollider = Physics2D.OverlapCircle(transform.position, checkRadius, LayerMask.GetMask(goonLayer));
        if (goonCollider) {
            if (goonCollider.GetComponent<PointStateMachine>().GetCurrentState() ==
                PointStateMachine.PointStates.Placed) {
                Debug.Log("Win !");
            }
        }
        
    }

    public void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector3.down * checkRadius, Color.red);
    }
}
