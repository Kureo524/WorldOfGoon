using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointContext {
    public GameObject PointObject;
    public PointStateMachine Self;
    public float DraggedLerpSpeed;
    public float MouseHoverRadius;

    public float DragRadius;
    public List<Link> Links = new();
    public Link CurrentDraggingLink;
    public UnityEvent OnInstantiateLink = new ();

    public Collider2D MousePosHit;
    
    public PointContext(GameObject pointObject, float dragRadius, float draggedLerpSpeed, GameObject linkToInstantiate, float mouseHoverRadius) {
        PointObject = pointObject;
        DragRadius = dragRadius;
        DraggedLerpSpeed = draggedLerpSpeed;
        MouseHoverRadius = mouseHoverRadius;
        Self = PointObject.GetComponent<PointStateMachine>();
    }
}
