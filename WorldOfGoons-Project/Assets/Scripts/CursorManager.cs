    using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    public static CursorManager Instance;
    
    public GameObject cursor;
    public Transform cursorParent;
    
    private GameObject _currentCursor;
    private LineRenderer _lineRenderer;
    
    private int _count = 0;
    private List<Vector2> _delayedVectors = new();

    private void Awake() {
        Cursor.lockState = CursorLockMode.Confined;
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Cursor.visible = false;
        _currentCursor = Instantiate(
            cursor,
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Quaternion.identity,
            cursorParent);
        _lineRenderer = _currentCursor.GetComponent<LineRenderer>();
    }

    private void FixedUpdate() {
        _currentCursor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _delayedVectors.Add(_currentCursor.transform.position);
        if (_delayedVectors.Count > 60) {
            _delayedVectors.RemoveAt(0);
        }

        for (int i = _lineRenderer.positionCount - 1; i >= 0 ; i--) {
            _lineRenderer.SetPosition(i, _delayedVectors.Count > i ? _delayedVectors[i] : _delayedVectors[^1]);
        }
    }
    
    public void ChangeCursor(GameObject newCursor) {    
        cursor = newCursor;
    }

    public void SetCursorVisibility(bool isVisible) {
        _currentCursor.GetComponent<SpriteRenderer>().enabled = isVisible;
    }
}
