using UnityEngine;

public class PointStateMachine : StateManager<PointStateMachine.PointStates> {
    public enum PointStates
    {
        Idle,
        Flying,
        Hover,
        Dragged,
        Placed,
        Sliding,
    }

    private void Awake() {
        _context = new PointContext(this, goonsLayerMask, dragRadius, dragLerpSpeed, gravityEffect,
            flyingCheckRadius, linkLayerMask, beeIdleSpeed, slidingSpeed,
            linkToInstantiate, linkParent);
        InitializeStates();
    }

    private void Start() {
         if(CurrentState.StateKey == PointStates.Placed)
             InitializeConnections();
    }
    
    public PointStates GetCurrentState()
    {
        return CurrentState.StateKey;
    }
    
    #region Variables
    
    private PointContext _context;

    public float beeIdleSpeed;
    public float slidingSpeed;

    [Header("Starting Values")] 
    public PointStates startingState;

    [Header("Drag Values")] 
    public float dragLerpSpeed;
    public float gravityEffect;
    
    [Header("Checks Values")] 
    public float dragRadius;
    public float flyingCheckRadius;
    public string goonsLayerMask;
    public string linkLayerMask;
    
    [Header("Link Values")] 
    public GameObject linkToInstantiate;
    public Transform linkParent;
    public Color linkColor;
    int segments = 60;
    
    #endregion
    
    #region Initialization Methods
    public void InitializeConnections() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            dragRadius,
            LayerMask.GetMask("GoonMachine"));
    
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
    
            if (!hit.TryGetComponent(out PointStateMachine otherPoint)) continue;
            if (otherPoint.GetCurrentState() != PointStates.Placed) continue;
    
            if (LinkExists(otherPoint)) continue;
    
            otherPoint.AddLink(this, _context.CreateLink(otherPoint));
            _context.SetLinksToPoints();
        }
    }
    
    private bool LinkExists(PointStateMachine otherPoint) {
        return _context.IsPointConnected(otherPoint);
    }
    
    public virtual void InitializeStates() {
        States.Add(PointStates.Idle, new PointIdleState(PointStates.Idle, _context));
        States.Add(PointStates.Flying, new PointFlyingState(PointStates.Flying, _context));
        States.Add(PointStates.Hover, new PointHoveredState(PointStates.Hover, _context));
        States.Add(PointStates.Dragged, new PointDraggedState(PointStates.Dragged, _context));
        States.Add(PointStates.Placed, new PointPlacedState(PointStates.Placed, _context));
        States.Add(PointStates.Sliding, new PointSlidingState(PointStates.Sliding, _context));
        CurrentState = States[startingState];
    }
    #endregion

    public void AddLink(PointStateMachine point, Link link) {
        _context.AddLink(point, link);
    }
    public void RemoveLink(PointStateMachine point) {
        _context.RemoveLink(point);
    }
    public void RemoveJoint(PointStateMachine point) {
        _context.RemoveJoint(point);
    }

    public void SetMouseHover(bool isHover) {
        Debug.Log("Hover : " + isHover);
        _context.IsMouseHover = isHover;
    }

    public void SetMousePosition(Vector2 position) {
        _context.MousePosition = position;
    }

    public void SetClicked(bool isClicked) {
        _context.IsClicked = isClicked;
    }

    public bool IsJointInPoint(PointStateMachine point) {
        return _context.IsJointInPoint(point);
    }

    public PointStateMachine GetRandomConnectedPoint() {
        return _context.GetRandomConnectedPoint();
    }
}