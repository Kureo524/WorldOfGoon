using UnityEngine;

public class PointFlyingState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointFlyingState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
