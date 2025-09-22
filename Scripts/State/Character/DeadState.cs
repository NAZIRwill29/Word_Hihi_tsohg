using UnityEngine;

[CreateAssetMenu(fileName = "DeadState", menuName = "State/CharacterStates/DeadState")]
public class DeadState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.Animator.SetBool("Dead", true);
        //objectT.ThingHappen(new() { SoundName = "Melee" });
        Debug.Log("enter Dead");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (!objectT.IsAlive) return;
        if (objectT.ObjectHealth.IsRevive)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Revive"]);
        }
    }

    public override void Exit(ObjectT objectT)
    {
        base.Exit(objectT);
        objectT.Animator.SetBool("Dead", false);
    }
}
