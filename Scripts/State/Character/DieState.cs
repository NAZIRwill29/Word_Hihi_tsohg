using UnityEngine;

[CreateAssetMenu(fileName = "DieState", menuName = "State/CharacterStates/DieState")]
public class DieState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.ObjectHealth.IsDie = false;
        objectT.Animator.SetTrigger("Die");
        //objectT.ThingHappen(new() { SoundName = "Melee" });
        Debug.Log("enter Die");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (!objectT.IsAlive)
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Dead"]);
    }
}
