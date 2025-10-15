using UnityEngine;

public class PointHoveredState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointHoveredState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
    }

    public override void ExitState() {
    }

    public override void UpdateState() {
        if (!_context.IsMouseHover) {
            NextState = PointStateMachine.PointStates.Sliding;
        }

        if (_context.IsClicked) {
            NextState = PointStateMachine.PointStates.Dragged;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
