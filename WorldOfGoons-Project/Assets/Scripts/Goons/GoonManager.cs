using System;
using System.Collections.Generic;
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
    
    void OnEnable() {
        GameController.Instance.onClick.AddListener(OnClick_Implementation);
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
                currentHoveredGoon.UpdateGoonState(GoonState.None);
            }
            
            currentHoveredGoon = hit.collider.GetComponent<Goon>();
            currentHoveredGoon.UpdateGoonState(GoonState.Hover);
        } else {
            if (currentHoveredGoon) {
                currentHoveredGoon.UpdateGoonState(GoonState.None);
                currentHoveredGoon = null;
            }
        }
    }
    private void DragGoon() { 
        currentDraggedGoon.transform.position = (Vector2)gameCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnClick_Implementation(bool isGoonDragged)
    {
        _isGoonDragged = isGoonDragged;
        if (currentHoveredGoon) {
            currentDraggedGoon = _isGoonDragged ? currentHoveredGoon : null;
            CursorManager.Instance.SetCursorVisibility(!isGoonDragged);
        }
    }
    
    #endregion
}
