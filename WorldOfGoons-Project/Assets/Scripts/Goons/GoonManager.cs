using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GoonManager : MonoBehaviour {
    #region Variables

    public Camera gameCamera;
    
    [Header("Goon Selector")] 
    public Goon currentHoveredGoon;
    public float goonSelectorRadius = 0.2f;

    [Header("Goon Dragger")] 
    public Goon currentDraggedGoon;
    
    private bool _isGoonDragged = false;
    
    #endregion

    #region Unity Methods
    
    void Start() {
        GameController.Instance.onClick.AddListener(OnClick_Implementation);
    }
    private void OnEnable() {
        if (GameController.Instance != null) {
            GameController.Instance.onClick.AddListener(OnClick_Implementation);
        }
    }
    void OnDisable() {
        GameController.Instance.onClick.RemoveListener(OnClick_Implementation);
    }
    private void Update() {
        if (currentDraggedGoon) {
            DragGoon();
        } else {
            GetClosestGoon();
        }
    }
    
    #endregion
    
    #region Methods

    private void GetClosestGoon() {
        RaycastHit2D hit;
        if (hit = Physics2D.CircleCast(gameCamera.ScreenToWorldPoint(Input.mousePosition), goonSelectorRadius, Vector2.zero, 0f,
                LayerMask.GetMask("Goon"))) {
            if (currentHoveredGoon && hit.collider.gameObject == currentHoveredGoon.gameObject) return;
            if (currentHoveredGoon) {
                currentHoveredGoon.UpdateGoonState(GoonState.Idle);
            }
            
            currentHoveredGoon = hit.collider.GetComponent<Goon>();
            currentHoveredGoon.UpdateGoonState(GoonState.Hover);
        } else {
            if (currentHoveredGoon) {
                currentHoveredGoon.UpdateGoonState(GoonState.Idle);
                currentHoveredGoon = null;
            }
        }
    }

    private void DragGoon() {
        currentDraggedGoon.currentState = GoonState.Dragged;
        currentDraggedGoon.transform.position = (Vector2)gameCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    

    private Dictionary<Vector2, Goon> _linkableGoons = new();
    private Dictionary<Vector2, TempGoonLink> _tempLinks = new();
    public GameObject tempLinkToInstantiate;
    public Transform tempLinkParent;
    private void CheckForTempLinks(Collider2D[] goonColliders) {
        List<Vector2> temp = new List<Vector2>();
        foreach (Collider2D goonCollider in goonColliders) {
            if(goonCollider.gameObject == currentDraggedGoon.gameObject) continue;
            temp.Add(goonCollider.transform.position);
            
            if (_linkableGoons.ContainsKey(goonCollider.transform.position)) {
                UpdateTempLinks(goonCollider.transform.position);
            } else {
                _linkableGoons.Add(goonCollider.transform.position, goonCollider.GetComponent<Goon>());
                TempGoonLink tempLink = Instantiate(tempLinkToInstantiate, tempLinkParent).GetComponent<TempGoonLink>();
                _tempLinks.Add(goonCollider.transform.position, tempLink);
                tempLink.startPosition = goonCollider.transform.position;
                tempLink.UpdateCreatingLink(currentDraggedGoon.transform.position);
            }
        }
        List<Vector2> tempLinks = _tempLinks.Keys.ToList();
        foreach (Vector2 tempLink in temp) {
            if (tempLinks.Contains(tempLink)) {
                tempLinks.Remove(tempLink);
            }
        }

        if (tempLinks.Count > 0) {
            foreach (Vector2 tempLink in tempLinks) {
                Destroy(_tempLinks[tempLink].gameObject);
                _tempLinks.Remove(tempLink);
                _linkableGoons.Remove(tempLink);
            }
        }
    }

    private void UpdateTempLinks(Vector2 checkPosition) {
        _tempLinks[checkPosition].UpdateCreatingLink(currentDraggedGoon.transform.position);
        Debug.Log("Already in linked : " + _tempLinks[checkPosition]);
    }
    
    private void OnClick_Implementation(bool isGoonDragged)
    {
        _isGoonDragged = isGoonDragged;
        if (currentHoveredGoon) {
            if (!_isGoonDragged && currentDraggedGoon != null) {
                currentDraggedGoon.onLinkableGoons.RemoveListener(CheckForTempLinks);
            }
            currentDraggedGoon = _isGoonDragged ? currentHoveredGoon : null;
            if (_isGoonDragged && currentDraggedGoon != null) {
                currentDraggedGoon.onLinkableGoons.AddListener(CheckForTempLinks);
            }
            CursorManager.Instance.SetCursorVisibility(!isGoonDragged);
        }
    }
    
    #endregion
}
