using UnityEngine;

[CreateAssetMenu(fileName = "ItemDestroyState", menuName = "State/ItemStates/ItemDestroyState")]
public class ItemDestroyState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.ObjectHealth.IsDie = false;
        objectT.Animator.SetTrigger("Destroy");
        //objectT.ThingHappen(new() { SoundName = "Melee" });
        Debug.Log("enter Destroy");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT.IsAlive) return;
        //if (StateTransitionItem.TransitionActive(objectT)) return;
        objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Destroyed"]);
    }

    public override void Exit(ObjectT objectT)
    {
    }
}
