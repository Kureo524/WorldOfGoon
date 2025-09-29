using System;
using UnityEngine;
using UnityEngine.Events;

public class TempGoonLink : MonoBehaviour {
    public Vector2 startPosition;
    public SpriteRenderer linkSpriteRenderer;

    public float maxLength;

    public UnityEvent<TempGoonLink> onDestroy;

    public bool UpdateCreatingLink(Vector2 toPosition) {
        transform.position = (toPosition + startPosition) / 2;
        Vector2 dir = toPosition - startPosition;
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        float length = dir.magnitude;
        linkSpriteRenderer.size = new Vector2(length, linkSpriteRenderer.size.y);

        if (length > maxLength) {
            onDestroy.Invoke(this);
            return false;
        }

        return true;
    }
}
