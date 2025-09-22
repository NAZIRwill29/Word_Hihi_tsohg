using UnityEngine;

[CreateAssetMenu(fileName = "HealState", menuName = "State/CharacterStates/HealState")]
public class HealState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.ObjectHealth.IsHeal = false;
        objectT.Animator.SetTrigger("Heal");
        //objectT.ThingHappen(new() { SoundName = "Melee" });
        Debug.Log("enter Heal");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT.ObjectHealth.IsHeal) return;
        if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
        //if (StateTransition.TransitionHealthNegative(objectT)) return;
        //if (StateTransition.TransitionDueAction(objectT)) return;
        StateTransitionCharacter.TransitionSpeed(objectT);
    }
}
