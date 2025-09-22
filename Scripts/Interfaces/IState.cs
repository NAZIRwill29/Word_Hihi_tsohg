using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(ObjectT objectT)
    {
        // code that runs when we first enter the state
    }

    public void Execute(ObjectT objectT)
    {
        // per-frame logic, include condition to transition to a new state
    }

    public void Exit(ObjectT objectT)
    {
        // code that runs when we exit the state
    }
}
