using UnityEngine;

public class PointIdleState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointIdleState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        _context.Self.GetComponent<SpriteRenderer>().color = Color.black;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        if (_context.IsMouseHover) {
            NextState = PointStateMachine.PointStates.Hover;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
