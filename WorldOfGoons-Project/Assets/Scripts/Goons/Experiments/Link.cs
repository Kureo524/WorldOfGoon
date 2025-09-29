using System;
using UnityEngine;

public class Link : MonoBehaviour {
    public Vector2 startPosition;
    public SpriteRenderer linkSpriteRenderer;

    public bool isPhysic;

    public void UpdateCreatingLink(Vector2 toPosition) {
        transform.position = (toPosition + startPosition) / 2;
        Vector2 dir = toPosition - startPosition;
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float length = dir.magnitude;
        linkSpriteRenderer.size = new Vector2(length, linkSpriteRenderer.size.y);
    }

    public void ActivatePhysic() {
        isPhysic = true;
        Debug.Log("Activated Physic");
    }

    public void DestroySelf() {
        Debug.Log("DestroySelf");
        Destroy(gameObject);
    }
}
