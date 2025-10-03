using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PointContext {
    public GameObject PointObject;
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



    public PointStateMachine Self;
    public Collider2D[] GetInRangeGoons(Vector2 position, float radius, int layerMask) {
        return Physics2D.OverlapCircleAll(position, radius, layerMask);
    }
    public Collider2D GetClosestGoons(Vector2 position, float radius, int layerMask) {
        return Physics2D.OverlapCircle(position, radius, layerMask);
    }

    private Link _tempLink;
    private GameObject _linkToInstantiate;
    private Transform _linksParent;
    public void CreateLink() {
        _tempLink = Object.Instantiate(_linkToInstantiate, _linksParent).GetComponent<Link>();
        _tempLink.startPoint = Self;
    }
    public void DestroyTempLink() {
        _tempLink.DestroySelf();
        _tempLink = null;
    }

    private List<Link> _links;
    public void AddLinkToList(Link link) {
        _links.Add(link);
    }
    public bool IsLinkInList(Link link) {
        return _links.Contains(link);
    }
    public void RemoveAllLinks() {
        for (int i = _links.Count - 1; i >= 0; i--) {
            _links[i].DestroySelf();
        }
    }
}
