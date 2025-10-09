using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour {
    public static GameController Instance;
    private void Awake() {
        Instance = this;
    }

    public UnityEvent<bool> onClick;
    public void OnClick(InputValue value) { 
        onClick.Invoke(value.isPressed);
    }
    
    public UnityEvent onEscape;
    public void OnEscape(InputValue value) {
        onEscape.Invoke(); 
    }

    public UnityEvent<Vector2> onMouseMove;
    public void OnMousePosition(InputValue value) {
        onMouseMove.Invoke(value.Get<Vector2>());
    }
}
