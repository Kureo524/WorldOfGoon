using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointContext {
    public PointContext(PointStateMachine self, string goonsLayerMask, float draggedRadius, float draggedLerpSpeed, float gravityEffect,
        GameObject linkToInstantiate, Transform linksParent) {
        Self = self;
        Body = Self.GetComponent<Rigidbody2D>(); 
        
        GoonsLayerMask = LayerMask.GetMask(goonsLayerMask);
        DraggedRadius = draggedRadius;
        DraggedLerpSpeed = draggedLerpSpeed;
        GravityEffect = gravityEffect;
        
        _linkToInstantiate = linkToInstantiate;
        _linksParent = linksParent;
    }

    #region Random Variables

    public PointStateMachine Self;
    public Rigidbody2D Body;
    
    public float DraggedLerpSpeed;
    public float GravityEffect;
    public Vector2 MousePosition;

    public bool IsMouseHover;
    public bool IsClicked;
    
    #endregion

    #region Checks
    
    public int GoonsLayerMask;
    public float DraggedRadius;
    
    public Collider2D[] GetInRangeGoons(Vector2 position, float radius, int layerMask) {
        return Physics2D.OverlapCircleAll(position, radius, layerMask);
    }
    public Collider2D GetClosestGoons(Vector2 position, float radius, int layerMask) {
        return Physics2D.OverlapCircle(position, radius, layerMask);
    }
    
    #endregion

    #region Links
    
    private Link _tempLink;
    private Dictionary<PointStateMachine , Link> _links = new();
    private GameObject _linkToInstantiate;
    private Transform _linksParent;
    
    public Link CreateLink(PointStateMachine otherPoint) {
        if (_links.ContainsKey(otherPoint)) {
            return null;
        }
        
        Link tempLink = Object.Instantiate(_linkToInstantiate, _linksParent).GetComponent<Link>();
        tempLink.InitializeValues(Self, otherPoint);
        tempLink.UpdatePosition();
        AddLink(otherPoint, tempLink);
        return tempLink;
    }
    public void AddLink(PointStateMachine otherPoint, Link link) {
        _links.TryAdd(otherPoint, link);
    }
    public void RemoveLink(PointStateMachine otherPoint) {
        if(_links.ContainsKey(otherPoint))
            _links.Remove(otherPoint);
    }

    public void ClearLinks() {
        _links.Clear();
    }
    public int GetLinksAmount() {
        return _links.Count;
    }
    public void DestroyTempLink(PointStateMachine connectedPoint) {
        Object.Destroy(_links[connectedPoint].gameObject);
    }
    public void UpdateTempLinkPositions() {
        List<PointStateMachine> pointsToRemove = new List<PointStateMachine>();
        foreach (Link link in _links.Values) {
            if (link != null) {
                link.UpdatePosition();
            }else {
                foreach (PointStateMachine point in _links.Keys) {
                    if (_links[point] == null) {
                        pointsToRemove.Add(point);
                    }
                }
            }
        }
        RemoveLinks(pointsToRemove);
    }
    public bool IsPointConnected(PointStateMachine connectedPoint) {
        return _links.ContainsKey(connectedPoint);
    }
    public List<PointStateMachine> GetConnectedPoints(Collider2D[] pointColliders) {
        List<PointStateMachine> connectedPoints = new();
        foreach (Collider2D pointCollider in pointColliders) {
            pointCollider.gameObject.TryGetComponent<PointStateMachine>(out PointStateMachine point);
            if(point == Self) continue;
            connectedPoints.Add(point);
        }
        return connectedPoints;
    }
    public List<PointStateMachine> GetLinksToRemove(List<PointStateMachine> otherPoints) {
        List<PointStateMachine> linksToRemove = new();
        foreach (PointStateMachine point in _links.Keys) {
            if(otherPoints.Contains(point))
                continue;
            linksToRemove.Add(point);
        }
        return linksToRemove;
    }
    public void RemoveLinks(List<PointStateMachine> linksToRemove) {
        if (linksToRemove.Count == 0) return;
        for (int i = linksToRemove.Count - 1; i >= 0; i--) {
            if(_links[linksToRemove[i]] != null)
                Object.Destroy(_links[linksToRemove[i]].gameObject);
            _links.Remove(linksToRemove[i]);
        }
    }

    private Dictionary<PointStateMachine, SpringJoint2D> _joints = new();
    public void SetLinksToPoints() {
        foreach (PointStateMachine point in _links.Keys) {
            point.AddLink(Self, _links[point]);
            if (!_joints.ContainsKey(point) && !point.IsJointInPoint(Self)) {
                SpringJoint2D joint = Self.AddComponent<SpringJoint2D>();
                joint.connectedBody = point.GetComponent<Rigidbody2D>();
                joint.dampingRatio = 1;
                joint.frequency = 25;
                _joints.Add(point, joint);
            }
        }
    }
    public void RemoveAllLinks() {
        foreach (PointStateMachine point in _links.Keys) {
            if (_joints.ContainsKey(point)) {
                RemoveJoint(point);
            } else {
                point.RemoveJoint(Self);
            }
        }
    }
    public void RemoveJoint(PointStateMachine point) {
        if (_joints.ContainsKey(point)) {
            Object.Destroy(_joints[point]);
            _joints.Remove(point);
        }
    }

    public bool IsJointInPoint(PointStateMachine point) {
        return _joints.ContainsKey(point);
    }
    
    #endregion
}