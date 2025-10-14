using UnityEngine;

public class PointFlyingState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;

    private float _timer;
    
    public PointFlyingState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        Debug.Log("entered flying state");
        _context.Body.bodyType = RigidbodyType2D.Dynamic;
        _timer = 0f;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        if (_context.Body.linearVelocity.magnitude > _context.FlyingDesiredVelocity) {
            _context.Body.linearVelocity = Vector2.MoveTowards(_context.Body.linearVelocity,
                _context.Body.linearVelocity.normalized * _context.FlyingDesiredVelocity,
                _context.FlyingDecreasingSpeed * Time.deltaTime
                );
        }

        if (_timer < _context.FlyingTime) {
            _timer += Time.deltaTime;
        } else {
            NextState = PointStateMachine.PointStates.Idle;
        }

        if (_context.IsClicked) {
            NextState = PointStateMachine.PointStates.Dragged;
        }
        if (_context.Body.linearVelocity == Vector2.zero) {
            NextState = PointStateMachine.PointStates.Idle;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
