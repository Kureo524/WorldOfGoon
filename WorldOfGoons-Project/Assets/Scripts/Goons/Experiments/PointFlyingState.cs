using UnityEngine;

public class PointFlyingState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointFlyingState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        Debug.Log("entered flying state");
        _context.Body.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        if (_context.GetClosestGoons(_context.Self.transform.position, _context.FlyingRadiusDetection, _context.LinkMask)) {
            // NextState = PointStateMachine.PointStates.Sliding;
        } else if (_context.Body.linearVelocity == Vector2.zero) {
            NextState = PointStateMachine.PointStates.Idle;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
