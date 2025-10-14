using UnityEngine;

public class PointExitState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointExitState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        if (_context.GoonPath.Count != 0) {
            _context.GoToGoon(_context.GoonPath[0], _context.ExitSpeed);
            Collider2D[] array;
            if ((array = _context.GetInRangeGoons(_context.Self.transform.position, _context.FlyingRadiusDetection / 8,
                    _context.GoonsLayerMask)).Length != 0) {
                foreach (var goon in array) {
                    goon.TryGetComponent(out PointStateMachine point);
                    if (point == _context.GoonPath[0]) {
                        _context.GoonPath.RemoveAt(0);
                    }
                }
            }
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
