using System;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    public float checkRadius;
    public string goonLayer;

    private float _timer;
    public float _timeToEnd;

    private PointStateMachine _connectedPoint;

    public OpenableCanvas endMenu;
    
    public void Update() {
        CheckForGoon();
        CheckTimer();
    }

    private void CheckTimer() {
        if (_timer >= _timeToEnd && _connectedPoint) {
            _connectedPoint.isAttachedToExit = true;
            EndLevel();
        }
    }

    private void CheckForGoon() {
        Collider2D goonCollider = Physics2D.OverlapCircle(transform.position, checkRadius, LayerMask.GetMask(goonLayer));
        if (goonCollider) {
            goonCollider.TryGetComponent(out PointStateMachine point);
            if (point.GetCurrentState() == PointStateMachine.PointStates.Placed) {
                _timer += Time.deltaTime;
                _connectedPoint = point;
            } else {
                ResetEnd();
            }
        } else {
            ResetEnd();
        }
    }

    private void ResetEnd() {
        _timer = 0;
        _connectedPoint = null;
    }

    private void EndLevel() {
        endMenu.OpenCanvas();
        Destroy(gameObject);
    }

    public void OnDrawGizmos() {
        Debug.DrawRay(transform.position, Vector3.down * checkRadius, Color.red);
    }
}
