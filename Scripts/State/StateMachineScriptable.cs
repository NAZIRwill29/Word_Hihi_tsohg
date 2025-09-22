using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptableStateData
{
    [SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : m_Name;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }
    public ScriptableState State;
}

[System.Serializable]
public class StateChangedData
{
    public ObjectT ObjectT;
    public ScriptableState State;
}

[CreateAssetMenu(fileName = "StateMachine", menuName = "StateMachines/StateMachine")]
public class StateMachineScriptable : ScriptableObject
{
    // Event to notify other objects of the state change
    public event Action<StateChangedData> StateChanged, Executed;
    public Dictionary<string, ScriptableState> StateDict = new Dictionary<string, ScriptableState>();
    [SerializeField] private ScriptableStateData[] m_StateDatas;
    private string m_CachedName;

    public void Initialize(ObjectT objectT)
    {
        StateDict.Clear();
        m_CachedName = name;

        if (m_StateDatas == null || m_StateDatas.Length == 0)
        {
            Debug.LogWarning($"State machine '{m_CachedName}' has no states assigned.");
            return;
        }

        foreach (var stateData in m_StateDatas)
        {
            if (stateData != null && stateData.State != null)
            {
                StateDict[stateData.Name] = stateData.State;
            }
            else
            {
                Debug.LogWarning($"Invalid state data found in '{m_CachedName}'. Ensure all entries are assigned.");
            }
        }

        // Set initial state
        if (StateDict.TryGetValue(m_StateDatas[0].Name, out ScriptableState initialState))
        {
            TransitionTo(objectT, initialState);
        }
        else
        {
            Debug.LogWarning($"Initial state '{m_StateDatas[0].Name}' not found in state machine '{m_CachedName}'.");
        }
    }

    public void TransitionTo(ObjectT objectT, ScriptableState nextState)
    {
        if (nextState == null)
        {
            Debug.LogWarning("Attempted to transition to a null state!");
            return;
        }

        if (objectT.ObjectStateView.CurrentState != null)
        {
            objectT.ObjectStateView.CurrentState.Exit(objectT);
        }

        objectT.ObjectStateView.CurrentState = nextState;
        objectT.ObjectStateView.CurrentState.Enter(objectT);

        // Get a reusable object from the pool
        StateChangedData stateChangedData = GenericObjectPool<StateChangedData>.Get();
        stateChangedData.ObjectT = objectT;
        stateChangedData.State = nextState;

        StateChanged?.Invoke(stateChangedData);

        // Return the object to the pool for reuse
        GenericObjectPool<StateChangedData>.Return(stateChangedData, data =>
        {
            data.ObjectT = null; // Reset values before returning to the pool
            data.State = null;
        });
    }

    // public void TransitionTo(ObjectT objectT, ScriptableState nextState)
    // {
    //     if (nextState == null)
    //     {
    //         Debug.LogWarning("Attempted to transition to a null state!");
    //         return;
    //     }

    //     if (objectT.ObjectStateView.CurrentState != null)
    //     {
    //         objectT.ObjectStateView.CurrentState.Exit(objectT);
    //     }

    //     objectT.ObjectStateView.CurrentState = nextState;
    //     objectT.ObjectStateView.CurrentState.Enter(objectT);

    //     StateChanged?.Invoke(new StateChangedData { ObjectT = objectT, State = nextState });
    // }

    public void Execute(ObjectT objectT)
    {
        //Debug.Log("Execute");
        Executed?.Invoke(new StateChangedData { ObjectT = objectT });
    }
}

