using System;
using UnityEngine;

public class Goon : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    
    public bool isGoonPlaced;

    public void UpdateGoonState(GoonState goonState) {
        switch (goonState) {
            case GoonState.None:
                spriteRenderer.color = Color.white;
                break;
            case GoonState.Hover:
                spriteRenderer.color = Color.red;
                break;
            case GoonState.Dragged:
                spriteRenderer.color = Color.green;
                break;
        }
    }
}

public enum GoonState {
    None,
    Hover,
    Dragged,
}
