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
        if (_context.ResetGoon) {
            _goonToGoTo = _context.GetRandomGoonToGoTo(_context.FlyingRadiusDetection);
        }
        _context.ResetGoon = false;
    }
    
    public override void ExitState() {
        Debug.Log($"Exit PointSlidingState");
    }

    public override void UpdateState() {
        if (_context.IsMouseHover) {
            NextState = PointStateMachine.PointStates.Hover;
            return;
        }
        // float magnitude = _context.GoToGoon(_goonToGoTo, _context.SlidingSpeed);
        float magnitude = _context.SlideOnLink(_goonToGoTo, _context.SlidingSpeed);
        if (magnitude <0) {
            NextState = PointStateMachine.PointStates.Idle;
        } else {
            if (magnitude < _context.FlyingRadiusDetection / 8) {
                _goonToGoTo = _context.GetRandomGoonToGoTo(_context.FlyingRadiusDetection);
            }
        }

        if (_goonToGoTo && _goonToGoTo.GetCurrentState() != PointStateMachine.PointStates.Placed) {
            NextState = PointStateMachine.PointStates.Idle;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
