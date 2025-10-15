using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIControls : MonoBehaviour {
    public UnityEvent onReturn;
    public void OnReturn(InputValue value) {
        onReturn.Invoke();
    }
}
