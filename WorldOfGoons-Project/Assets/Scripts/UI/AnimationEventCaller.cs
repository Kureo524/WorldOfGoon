using UnityEngine;
using UnityEngine.Events;

public class AnimationEventCaller : MonoBehaviour
{
    public UnityEvent tickedEvent;
    public void OnTicked() {
        tickedEvent.Invoke();
    }
}
