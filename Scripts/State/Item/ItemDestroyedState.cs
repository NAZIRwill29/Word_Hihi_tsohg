using UnityEngine;

[CreateAssetMenu(fileName = "ItemDestroyedState", menuName = "State/ItemStates/ItemDestroyedState")]
public class ItemDestroyedState : ScriptableState
{
    [SerializeField] private bool IsInActive;
    // [SerializeField] private bool IsDeadInActive;
    public override void Enter(ObjectT objectT)
    {
        objectT.Animator.SetBool("Destroyed", true);
        Debug.Log("enter Destroyed");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        //if (!objectT.IsAlive) return;
        if (objectT.ObjectHealth.IsRevive)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Revive"]);
        }
        if (IsInActive)
        {
            StateTransitionItem.TransitionActive(objectT);
            //if () return;
            if (objectT is ItemObjectT itemObjectT)
                itemObjectT.Deactivate();
        }
        // if (IsDeadInActive && objectT is ItemObjectT itemObjectT)
        //     itemObjectT.Deactivate();
    }

    public override void Exit(ObjectT objectT)
    {
        objectT.Animator.SetBool("Destroyed", false);
    }
}