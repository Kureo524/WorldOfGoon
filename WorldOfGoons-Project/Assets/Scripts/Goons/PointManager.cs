using UnityEngine;

public class PointManager : MonoBehaviour {
    private PointStateMachine _currentGoon;

    public float pointSelectionRadius;

    private RaycastHit2D _hit;

    private void Start() {
        GameController.Instance.onClick.AddListener(OnMouseClick);
    }

    private void Update() {
        if (_currentGoon && _currentGoon.GetCurrentState() == PointStateMachine.PointStates.Dragged) return;
        
        if (_hit = Physics2D.CircleCast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                pointSelectionRadius,
                Vector2.zero,
                0f,
                LayerMask.GetMask("GoonMachine"))) {
            if (_currentGoon && _hit.collider.gameObject == _currentGoon.gameObject && _currentGoon.GetCurrentState() != PointStateMachine.PointStates.Idle) return;
            if (_currentGoon && _currentGoon.GetCurrentState() != PointStateMachine.PointStates.Idle) {
                _currentGoon.UpdateCurrentState(PointStateMachine.PointStates.Idle);
            }
            _currentGoon = _hit.collider.GetComponent<PointStateMachine>();
            if (_currentGoon.GetCurrentState() == PointStateMachine.PointStates.Flying) return;
            _currentGoon.UpdateCurrentState(PointStateMachine.PointStates.Hover);
        } else {
            if (!_currentGoon) return;
            _currentGoon.UpdateCurrentState(PointStateMachine.PointStates.Idle);
            _currentGoon = null;
        }
    }

    private void OnMouseClick(bool isPressed) {
        if (_currentGoon) {
            if (isPressed && _currentGoon.GetCurrentState() == PointStateMachine.PointStates.Hover) {
                _currentGoon.UpdateCurrentState(PointStateMachine.PointStates.Dragged);
            } else if (!isPressed && _currentGoon.GetCurrentState() == PointStateMachine.PointStates.Dragged) {
                _currentGoon.UpdateCurrentState(PointStateMachine.PointStates.Idle);
            }
        }
    }
}
