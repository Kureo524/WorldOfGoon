using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointSlidingState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    private PointStateMachine _previousPoint;
    private PointStateMachine _goonToGoTo;
    
    public PointSlidingState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        _context.Body.bodyType = RigidbodyType2D.Kinematic;
        if(_context.ResetGoon)
            _goonToGoTo = _context.GetRandomGoonToGoTo(_context.FlyingRadiusDetection);
        _context.ResetGoon = false;
    }
    
    public override void ExitState() {
        
    }

    public override void UpdateState() {
        if (_context.IsMouseHover) {
            NextState = PointStateMachine.PointStates.Hover;
            return;
        }
        
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
