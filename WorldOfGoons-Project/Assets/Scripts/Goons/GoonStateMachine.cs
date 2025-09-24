using UnityEngine;

public class GoonStateMachine : StateManager<GoonStateMachine.GoonStates> {
    public enum GoonStates {
        None,
        Hovered,
        Dragged,
    }
    
    #region ContextVariables
    
    public SpriteRenderer goonSpriteRenderer;
    
    #endregion

    private GoonContext _context;

    private void Awake() {
        _context = new GoonContext(goonSpriteRenderer);
        InitializeStates();
    }

    private void InitializeStates() {
        States.Add(GoonStates.None, new GoonIdleState(GoonStates.None));
        States.Add(GoonStates.Hovered, new GoonHoveredState(GoonStates.Hovered));
        States.Add(GoonStates.Dragged, new GoonDraggedState(GoonStates.Dragged));
        CurrentState = States[GoonStates.None];
    }


}
