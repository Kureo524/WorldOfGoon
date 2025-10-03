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

    private void Awake() {
        _context = new PointContext(gameObject, dragRadius, dragLerpSpeed, linkToInstantiate, mouseHoverRadius);
        InitializeStates();
        _context.OnInstantiateLink.AddListener(CreateLink);
    }

    private void Start() {
        GameController.Instance.onClick.AddListener(SetIsClicked);
        // if(CurrentState.StateKey == PointStates.Placed)
        //     InitializeConnections();
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

    private void CreateLink() {
        _context.CurrentDraggingLink = Instantiate(linkToInstantiate, linkParent).GetComponent<Link>();
        _context.CurrentDraggingLink.GetComponent<Link>().startPoint = this;
    }
    
    private Link GiveCreateLink() {
        Link link = Instantiate(linkToInstantiate, linkParent).GetComponent<Link>();
        link.startPoint = this;
        return link;
    }

    public void AddLink(Link link) {
        if (!_context.Links.Contains(link)) {
            _context.Links.Add(link);
        }
    }
    
    public void InitializeConnections()
    {
        // Récupère tous les points voisins dans le rayon
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            _context.PointObject.transform.position,
            _context.DragRadius,
            LayerMask.GetMask("GoonMachine"));
    
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue; // ignore soi-même
    
            // Vérifie si c'est un Point valide
            if (!hit.TryGetComponent(out PointStateMachine otherPoint)) continue;
            if (otherPoint.GetCurrentState() != PointStates.Placed) continue;
    
            // Vérifie si un lien existe déjà entre "this" et "otherPoint"
            if (LinkExists(this, otherPoint)) continue;
    
            // Crée le lien
            Link newLink = GiveCreateLink();
            if (newLink != null)
            {
                newLink.UpdateCreatingLink(otherPoint);
    
                AddLink(newLink);
                otherPoint.AddLink(newLink);
            }
        }
    }
    
    /// <summary>
    /// Vérifie si un lien existe déjà entre deux points
    /// </summary>
    private bool LinkExists(PointStateMachine a, PointStateMachine b)
    {
        foreach (Link link in _context.Links)
        {
            if ((link.startPoint == a && link.endPoint == b) ||
                (link.startPoint == b && link.endPoint == a))
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateState(PointStates newState) {
        TransitionToState(newState);
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



    public void SetMouseHover(bool isHover) {
        Debug.Log("Hover : " + isHover);
    }
}