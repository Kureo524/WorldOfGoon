using System.Collections.Generic;
using UnityEngine;

public class PointDraggedState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointDraggedState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        _context.Body.bodyType = RigidbodyType2D.Kinematic;
        _context.ResetGoon = true;
    }

    public override void ExitState() {
        _context.Body.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void UpdateState()
    {
        Vector2 diff = _context.MousePosition - _context.Body.position;
        _context.Body.linearVelocity = diff * ((_context.DraggedLerpSpeed * 100) * Time.deltaTime);
        
        // _context.Body.linearVelocity = Vector2.Lerp(_context.Body.linearVelocity, _context.MousePosition - (Vector2)_context.Self.transform.position,_context.DraggedLerpSpeed * Time.deltaTime);
        
        // _context.Self.transform.position = Vector2.Lerp(_context.Self.transform.position, _context.MousePosition,
        //     _context.DraggedLerpSpeed * Time.deltaTime);

        List<PointStateMachine> points = _context.GetConnectedPoints(_context.GetInRangeGoons(
            _context.Self.transform.position, _context.DraggedRadius, _context.GoonsLayerMask));
        
        foreach (PointStateMachine otherPoint in points) {
            if(otherPoint.GetCurrentState() == PointStateMachine.PointStates.Placed)
                _context.CreateLink(otherPoint);
        }
        _context.RemoveLinks(_context.GetLinksToRemove(points));
        _context.UpdateTempLinkPositions();

        if (!_context.IsClicked) {
            NextState = _context.GetLinksAmount() != 0 ? PointStateMachine.PointStates.Placed : PointStateMachine.PointStates.Flying;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
