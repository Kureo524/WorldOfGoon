using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class GoonsCursorController : MonoBehaviour {
    public string goonLayer;
    public float checkRadius;

    public PointStateMachine currentHoveredGoon;
    
    private void Start() {
        GameController.Instance.onMouseMove.AddListener(TakeMousePosition);
    }

    private void TakeMousePosition(Vector2 onScreenPosition) {
        PointStateMachine currentGoon = GetClosestGoon<PointStateMachine>(Camera.main.ScreenToWorldPoint(onScreenPosition), LayerMask.GetMask(goonLayer));;
        if (currentGoon != currentHoveredGoon) {
            if(currentHoveredGoon)
                currentHoveredGoon.SetMouseHover(false);
            if(currentGoon)
                currentGoon.SetMouseHover(true);
            currentHoveredGoon = currentGoon;
        }
    }

    private T GetClosestGoon<T>(Vector2 mousePos, int layerMask) where T : Component {
        Collider2D current = Physics2D.OverlapCircle(mousePos, checkRadius, layerMask);
        return current ? Physics2D.OverlapCircle(mousePos, checkRadius, layerMask).GetComponent<T>() : null;
    }
}
