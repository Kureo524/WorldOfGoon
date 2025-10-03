using System;
using UnityEngine;

public class Link : MonoBehaviour {
    public PointStateMachine startPoint;
    public PointStateMachine endPoint;
    public SpriteRenderer linkSpriteRenderer;

    public bool isPhysic;

    public void UpdateCreatingLink(PointStateMachine toPoint) {
        endPoint = toPoint;
        transform.position = (toPoint.transform.position + startPoint.transform.position) / 2;
        Vector2 dir = toPoint.transform.position - startPoint.transform.position;
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

    public void DestroyLinkInEditor() {
        DestroyImmediate(gameObject);
    }
}
