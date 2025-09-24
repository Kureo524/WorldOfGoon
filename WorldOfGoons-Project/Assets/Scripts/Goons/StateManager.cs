using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    void Start() {
        CurrentState.EnterState();
    }

    void Update()
    {
        if(Time.timeScale == 0) return;
    
        EState _nextStateKey = CurrentState.GetNextState();

        if (_nextStateKey.Equals(CurrentState.StateKey))
            CurrentState.UpdateState();
        else
            TransitionToState(_nextStateKey);
    }

    protected void TransitionToState(EState _stateKey) {
        CurrentState.ExitState();
        CurrentState = States[_stateKey];
        CurrentState.EnterState();
    }
}

