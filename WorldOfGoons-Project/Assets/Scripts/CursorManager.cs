using UnityEngine;

public class CursorManager : MonoBehaviour {
    public static CursorManager Instance;
    
    public GameObject cursor;
    public Transform cursorParent;
    
    private GameObject _currentCursor;

    private void Awake() {
        Instance = this;
        Cursor.visible = false;
        _currentCursor = Instantiate(
            cursor,
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Quaternion.identity,
            cursorParent);
    }

    private void Update() {
        _currentCursor.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    public void ChangeCursor(GameObject newCursor) {
        cursor = newCursor;
    }

    public void SetCursorVisibility(bool isVisible) {
        _currentCursor.GetComponent<SpriteRenderer>().enabled = isVisible;
    }
}
