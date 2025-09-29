using UnityEngine;

public class PointDraggedState : BaseState<PointStateMachine.PointStates> {
    private PointContext _context;
    
    public PointDraggedState(PointStateMachine.PointStates _key, PointContext context) : base(_key) {
        _context = context;
    }

    public override void EnterState() {
        Debug.Log("Entered pointDraggedState");
        NextState = StateKey;
    }

    public override void ExitState() {
        
    }

    public override void UpdateState() {
        _context.PointObject.transform.position = Vector2.Lerp(_context.PointObject.transform.position,
            Camera.main.ScreenToWorldPoint(Input.mousePosition), _context.DraggedLerpSpeed * Time.deltaTime);
    }

    public override PointStateMachine.PointStates GetNextState() {
        return NextState;
    }
}
