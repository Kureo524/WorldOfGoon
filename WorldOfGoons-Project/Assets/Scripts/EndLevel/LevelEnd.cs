using System;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    public float checkRadius;
    public string goonLayer;

    private float _timer;
    public float _timeToEnd;
    
    public void Update() {
        CheckForGoon();
        CheckTimer();
    }

    private void CheckTimer() {
        if (_timer >= _timeToEnd) {
            Debug.Log("Win");
        }
    }

    private void CheckForGoon() {
        Collider2D goonCollider = Physics2D.OverlapCircle(transform.position, checkRadius, LayerMask.GetMask(goonLayer));
        if (goonCollider) {
            goonCollider.TryGetComponent(out PointStateMachine point);
            if (point.GetCurrentState() == PointStateMachine.PointStates.Placed) {
                _timer += Time.deltaTime;
            } else {
                ResetEnd();
            }
        } else {
            ResetEnd();
        }
    }

    private void ResetEnd() {
        _timer = 0;
    }

    public void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector3.down * checkRadius, Color.red);
    }
}
