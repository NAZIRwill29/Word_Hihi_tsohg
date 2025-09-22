using System.Collections.Generic;
using UnityEngine;

public class ScriptableState : ScriptableObject, IState
{
    [SerializeField] protected string m_AnimationName;

    public virtual void Enter(ObjectT objectT)
    {
        // code that runs when we first enter the state
        //Debug.Log($"Entering State: {name}");
    }

    // per-frame logic, include condition to transition to a new state
    public virtual void Execute(ObjectT objectT)
    {
        // if (nextStateName + "State" == name)
        //     return;
        // if (NextStateDict.TryGetValue(nextStateName, out ScriptableState nextState))
        // {
        //     Debug.Log($"Transitioning from {name} to {nextState.name}");
        //     objectT.StateMachineScriptable.TransitionTo(objectT, nextState);
        // }
        // else
        //     Debug.LogWarning($"State '{name}' has no transition to '{nextStateName}'!");
    }

    public virtual void Exit(ObjectT objectT)
    {
        // code that runs when we exit the state
        //Debug.Log($"Exiting State: {name}");
        objectT.RefreshNatureElement();
    }
}
