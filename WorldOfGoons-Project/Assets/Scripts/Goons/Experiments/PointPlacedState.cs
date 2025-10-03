using UnityEngine;
using UnityEngine.Events;

public class PointPlacedState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;

    private Collider2D[] _hit;
    private PointStateMachine _point;
    
    public PointPlacedState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        NextState = StateKey;
        _context.CurrentDraggingLink = null;
    }

    public override void ExitState() {
        if (_context.Links.Count == 0) return;
        for (int i = _context.Links.Count - 1; i >= 0; i--) {
            if(_context.Links[i])
                _context.Links[i].DestroySelf();
        }
        _context.CurrentDraggingLink = null;
        _point = null;
    }

    public override void UpdateState() {
        _context.MousePosHit = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition),
            _context.MouseHoverRadius, LayerMask.GetMask("GoonMachine"));
        
        bool isLinking = false;
        if ((_hit = Physics2D.OverlapCircleAll(_context.PointObject.transform.position, _context.DragRadius,
                LayerMask.GetMask("GoonMachine"))).Length != 0) {
            foreach (Collider2D hit in _hit) {
                if (hit.GetComponent<PointStateMachine>().GetCurrentState() ==
                    PointStateMachine.PointStates.Dragged || hit.GetComponent<PointStateMachine>() == _point) {
                    isLinking = true;
                    if (hit.GetComponent<PointStateMachine>() == _point) {
                        if (_point.GetCurrentState() == PointStateMachine.PointStates.Placed) {
                            if (!_context.Links.Contains(_context.CurrentDraggingLink)) {
                                _context.Links.Add(_context.CurrentDraggingLink);
                            }
                            _point.AddLink(_context.CurrentDraggingLink);
                            _context.CurrentDraggingLink = null;
                            _point = null;
                        }
                    } else {
                        if(_point) _point.UpdateConnections(_context.Self, false);
                        _point = hit.GetComponent<PointStateMachine>();
                        _point.UpdateConnections(_context.Self, true);
                    }

                    if (!_context.CurrentDraggingLink) _context.OnInstantiateLink.Invoke();
                    else _context.CurrentDraggingLink.GetComponent<Link>().UpdateCreatingLink(_point);
                }
            }
        }

        if (isLinking == false && _context.CurrentDraggingLink) {
            if (_point) {
                if(_point) _point.UpdateConnections(_context.Self, false);
            }
            _context.CurrentDraggingLink.GetComponent<Link>().DestroySelf();
            _point = null;
            _context.CurrentDraggingLink = null;
        }
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
