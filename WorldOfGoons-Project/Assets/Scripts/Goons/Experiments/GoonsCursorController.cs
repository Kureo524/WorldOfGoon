using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class GoonsCursorController : MonoBehaviour {
    public string goonLayer;
    public float checkRadius;

    public PointStateMachine currentHoveredGoon;
    private bool _isGoonDragged;
    
    private Vector2 _mousePos;
    
    private void Start() {
        GameController.Instance.onMouseMove.AddListener(TakeMousePosition);
        GameController.Instance.onClick.AddListener(TakeClick);
    }

    private void TakeMousePosition(Vector2 onScreenPosition) {
        _mousePos = Camera.main.ScreenToWorldPoint(onScreenPosition);
        
        if (_isGoonDragged) {
            currentHoveredGoon.SetMousePosition(_mousePos);
            return;
        }
        
        PointStateMachine currentGoon = GetClosestGoon<PointStateMachine>(_mousePos, LayerMask.GetMask(goonLayer));;
        if (currentGoon != currentHoveredGoon) {
            if(currentHoveredGoon)
                currentHoveredGoon.SetMouseHover(false);
            if(currentGoon)
                currentGoon.SetMouseHover(true);
            currentHoveredGoon = currentGoon;
        }
    }

    private void TakeClick(bool clicked) {
        if (!currentHoveredGoon) return;
        currentHoveredGoon.SetMousePosition(_mousePos);
        currentHoveredGoon.SetClicked(clicked);
        _isGoonDragged = clicked;
    }

    private T GetClosestGoon<T>(Vector2 mousePos, int layerMask) where T : Component {
        Collider2D current = Physics2D.OverlapCircle(mousePos, checkRadius, layerMask);
        return current ? Physics2D.OverlapCircle(mousePos, checkRadius, layerMask).GetComponent<T>() : null;
    }
}
