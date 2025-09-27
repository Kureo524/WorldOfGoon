using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Goon : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

    [Header("Dragged Variables")] 
    public float dragRadius;
    public Collider2D[] draggableGoons;
    
    private Dictionary<Vector2, Goon> _linkedGoons = new();
    
    public GoonState currentState = GoonState.Idle;

    public UnityEvent<Collider2D[]> onLinkableGoons;

    public void UpdateGoonState(GoonState goonState) {
        currentState = goonState;
        spriteRenderer.color = currentState switch
        {
            GoonState.Idle => Color.white,
            GoonState.Hover => Color.red,
            GoonState.Dragged => Color.green,
            GoonState.Placed => Color.blue,
            _ => spriteRenderer.color
        };
    }

    private void Update() {
        if (currentState == GoonState.Dragged) {
            if ((draggableGoons = Physics2D.OverlapCircleAll(transform.position, dragRadius, LayerMask.GetMask("Goon"))).Length != 0) {
                onLinkableGoons?.Invoke(draggableGoons);
            }
        }
    }
}

public enum GoonState {
    Idle,
    Hover,
    Dragged,
    Placed,
}
