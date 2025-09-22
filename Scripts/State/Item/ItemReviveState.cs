using UnityEngine;

[CreateAssetMenu(fileName = "ItemReviveState", menuName = "State/ItemStates/ItemReviveState")]
public class ItemReviveState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.ObjectHealth.IsRevive = false;
        objectT.Animator.SetTrigger("Revive");
        Debug.Log("enter Revive");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT.ObjectHealth.IsRevive) return;
        if (StateTransitionItem.TransitionHealthPositive(objectT)) return;
        StateTransitionItem.TransitionSpeed(objectT);
    }

    public override void Exit(ObjectT objectT)
    {
    }
}
