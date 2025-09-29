using System.Collections.Generic;
using UnityEngine;

public class PointStateMachine : StateManager<PointStateMachine.PointStates>
{
    private PointContext _context;

    [Header("Starting Values")] public PointStates startingState;
    public float mouseHoverRadius;

    [Header("Drag Values")] public float dragRadius;
    public float dragLerpSpeed;
    public GameObject linkToInstantiate;
    public Transform linkParent;

    public Dictionary<PointStateMachine, bool> HasConnections = new();

    public enum PointStates
    {
        Idle,
        Flying,
        Hover,
        Dragged,
        Placed,
    }

    private void Awake()
    {
        _context = new PointContext(gameObject, dragRadius, dragLerpSpeed, linkToInstantiate, mouseHoverRadius);
        InitializeStates();
        _context.OnInstantiateLink.AddListener(CreateLink);
    }

    private void Start() {
        GameController.Instance.onClick.AddListener(SetIsClicked);
    }

    public void UpdateCurrentState(PointStates newState)
    {
        TransitionToState(newState);
    }

    private void SetIsClicked(bool isPressed)
    {
        if (isPressed && CurrentState.StateKey == PointStates.Hover) {
            CurrentState.SetNextState(PointStates.Dragged);
        }
        else if (!isPressed && CurrentState.StateKey == PointStates.Dragged) {
            foreach (bool hasConnection in HasConnections.Values) {
                if (hasConnection) {
                    CurrentState.SetNextState(PointStates.Placed);
                    return;
                }
            }
            CurrentState.SetNextState(PointStates.Idle);
        } else if (isPressed && CurrentState.StateKey == PointStates.Placed && _context.MousePosHit &&
                   _context.MousePosHit.gameObject == _context.PointObject) {
            CurrentState.SetNextState(PointStates.Dragged);
        }
    }

    public PointStates GetCurrentState()
    {
        return CurrentState.StateKey;
    }

    private void CreateLink()
    {
        _context.CurrentDraggingLink = Instantiate(linkToInstantiate, linkParent).GetComponent<Link>();
        _context.CurrentDraggingLink.GetComponent<Link>().startPosition = transform.position;
    }

    public void AddLink(Link link) {
        if (!_context.Links.Contains(link)) {
            _context.Links.Add(link);
        }
    }

    public void UpdateConnections(PointStateMachine sender, bool isConnected) {
        if (HasConnections.ContainsKey(sender)) {
            HasConnections[sender] = isConnected;
        } else {
            HasConnections.Add(sender, isConnected);
        }
    }

    private void InitializeStates()
    {
        States.Add(PointStates.Idle, new PointIdleState(PointStates.Idle, _context));
        States.Add(PointStates.Flying, new PointFlyingState(PointStates.Flying, _context));
        States.Add(PointStates.Hover, new PointHoveredState(PointStates.Hover, _context));
        States.Add(PointStates.Dragged, new PointDraggedState(PointStates.Dragged, _context));
        States.Add(PointStates.Placed, new PointPlacedState(PointStates.Placed, _context));
        CurrentState = States[startingState];
    }
}