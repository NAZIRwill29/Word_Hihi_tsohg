using System;

// handles
[Serializable]
public class StateMachine
{
    public IState CurrentState { get; private set; }
    // event to notify other objects of the state change
    public event Action<IState> stateChanged;

    // set the starting state
    public void Initialize(ObjectT objectT, IState state)
    {
        CurrentState = state;
        state.Enter(objectT);

        // notify other objects that state has changed
        stateChanged?.Invoke(state);
    }

    // exit this state and enter another
    public void TransitionTo(ObjectT objectT, IState nextState)
    {
        CurrentState.Exit(objectT);
        CurrentState = nextState;
        nextState.Enter(objectT);

        // notify other objects that state has changed
        stateChanged?.Invoke(nextState);
    }

    // allow the StateMachine to update this state
    public void Execute(ObjectT objectT)
    {
        if (CurrentState != null)
        {
            CurrentState.Execute(objectT);
        }
    }
}