using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A user interface that responds to internal state changes
/// </summary>
//[RequireComponent(typeof(PlayerController))]
public class ObjectStateView : MonoBehaviour
{
    public ScriptableState CurrentState;//{ get; set; }

    [SerializeField] private TextMeshProUGUI m_LabelText;
    private ObjectT m_ObjectT;

    private void Awake()
    {
        m_ObjectT = GetComponent<ObjectT>();
    }

    private void Start()
    {
        if (m_ObjectT.StateMachineScriptable == null)
        {
            Debug.LogWarning("ObjectStateMachine is not initialized in ObjectT.");
            return;
        }

        // Subscribe to state change events
        m_ObjectT.StateMachineScriptable.StateChanged += OnStateChanged;
        m_ObjectT.StateMachineScriptable.Executed += OnExecuted;
    }

    void OnDestroy()
    {
        // Unsubscribe from state change events
        if (m_ObjectT != null && m_ObjectT.StateMachineScriptable != null)
        {
            m_ObjectT.StateMachineScriptable.StateChanged -= OnStateChanged;
            m_ObjectT.StateMachineScriptable.Executed -= OnExecuted;
        }
    }

    private void OnStateChanged(StateChangedData stateChangedData)
    {
        if (stateChangedData.ObjectT != m_ObjectT)
            return;

        CurrentState = stateChangedData.State;
        CurrentState.Enter(m_ObjectT);

        if (m_LabelText != null && stateChangedData.State is ScriptableState scriptableState)
        {
            m_LabelText.text = scriptableState.name;
        }
    }

    private void OnExecuted(StateChangedData stateChangedData)
    {
        if (stateChangedData.ObjectT != m_ObjectT)
            return;

        if (CurrentState != null)
        {
            CurrentState.Execute(m_ObjectT);
        }
        else
        {
            Debug.LogWarning("CurrentState is null in ObjectStateView!");
        }
    }
}
