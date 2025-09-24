using UnityEngine;

public class GoonDraggedState : BaseState<GoonStateMachine.GoonStates>
{
    public GoonDraggedState(GoonStateMachine.GoonStates _key) : base(_key) { }

    public override void EnterState() { }

    public override void ExitState() { }

    public override void UpdateState() { }

    public override GoonStateMachine.GoonStates GetNextState() { return NextState; }

    public override void OnTriggerEnter(Collider2D _other) { }

    public override void OnTriggerStay(Collider2D _other) { }

    public override void OnTriggerExit(Collider2D _other) { }

    public override void OnCollisionEnter(Collision2D _other) { }

    public override void OnCollisionStay(Collision2D _other) { }

    public override void OnCollisionExit(Collision2D _other) { }
}
