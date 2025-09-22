using UnityEngine;

[CreateAssetMenu(fileName = "ItemInactiveState", menuName = "State/ItemStates/ItemInactiveState")]
public class ItemInactiveState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is ItemObjectT itemObjectT)
        {
            itemObjectT.IsInActiveState = false;
            objectT.Animator.SetTrigger("InActive");
            //itemObjectT.Deactivate();
            Debug.Log("enter InActive");
            itemObjectT.InProgressInActiveState = false;
        }
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT is ItemObjectT itemObjectT && itemObjectT.IsInActiveState) return;
        if (StateTransitionItem.TransitionActive(objectT)) return;
    }

    public override void Exit(ObjectT objectT)
    {
    }
}