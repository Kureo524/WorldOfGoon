using UnityEngine;

public class PointIdleState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;

    private PointStateMachine _goonToGoTo;
    
    public PointIdleState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        if (_context.Body.linearVelocity != Vector2.zero) {
            NextState = PointStateMachine.PointStates.Flying;
        }
        _context.Body.bodyType = RigidbodyType2D.Kinematic;
        _goonToGoTo = _context.GetClosestGoonToGoTo();
    }

    public override void ExitState() {
        _context.Body.linearVelocity = Vector2.zero;
    }

    public override void UpdateState() {
        float magnitude = _context.GoToGoon(_goonToGoTo, _context.BeeIdleSpeed);
        if (magnitude <0) {
            _goonToGoTo = _context.GetClosestGoonToGoTo();
        } else {
            if (magnitude < _context.FlyingRadiusDetection / 2) {
                NextState = PointStateMachine.PointStates.Sliding;
            }
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
