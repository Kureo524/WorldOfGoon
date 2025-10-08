using UnityEngine;
using UnityEngine.Events;

public class PointPlacedState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;

    private Collider2D[] _hit;
    private PointStateMachine _point;
    
    public PointPlacedState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        _context.SetLinksToPoints();
    }

    public override void ExitState() {
        _context.RemoveAllLinks();
    }

    public override void UpdateState() {
        if(_context.IsClicked)
            NextState = PointStateMachine.PointStates.Dragged;
        
        _context.UpdateTempLinkPositions();
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
