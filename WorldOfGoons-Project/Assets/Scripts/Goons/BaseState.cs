using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    protected EState NextState;
    
    public BaseState(EState _key) {
        StateKey = _key;
        NextState = _key;
    }
    
    public EState StateKey { get; private set; }
    
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter(Collider2D _other);
    public abstract void OnTriggerStay(Collider2D _other);
    public abstract void OnTriggerExit(Collider2D _other);
    public abstract void OnCollisionEnter(Collision2D _other);
    public abstract void OnCollisionStay(Collision2D _other);
    public abstract void OnCollisionExit(Collision2D _other);
}
