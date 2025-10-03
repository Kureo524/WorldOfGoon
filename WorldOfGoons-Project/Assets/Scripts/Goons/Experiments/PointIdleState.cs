using UnityEngine;

public class PointIdleState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointIdleState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        _context.MousePosHit = null;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        _context.MousePosHit = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition),
            _context.MouseHoverRadius, LayerMask.GetMask("GoonMachine"));

        if(_context.MousePosHit && _context.MousePosHit.gameObject == _context.PointObject) {
            NextState = PointStateMachine.PointStates.Hover;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
