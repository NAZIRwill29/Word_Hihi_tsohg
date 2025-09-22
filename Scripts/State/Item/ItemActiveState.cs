using UnityEngine;

[CreateAssetMenu(fileName = "ItemActiveState", menuName = "State/ItemStates/ItemActiveState")]
public class ItemActiveState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is ItemObjectT itemObjectT)
        {
            itemObjectT.IsActiveState = false;
            objectT.Animator.SetTrigger("Active");
            Debug.Log("enter Active");
        }
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT is ItemObjectT itemObjectT && itemObjectT.IsActiveState) return;
        if (StateTransitionItem.TransitionHealthPositive(objectT)) return;
        StateTransitionItem.TransitionSpeed(objectT);
    }

    public override void Exit(ObjectT objectT)
    {
    }
}
