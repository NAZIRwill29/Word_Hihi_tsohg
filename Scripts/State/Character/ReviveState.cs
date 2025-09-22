using UnityEngine;

[CreateAssetMenu(fileName = "ReviveState", menuName = "State/CharacterStates/ReviveState")]
public class ReviveState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.ObjectHealth.IsRevive = false;
        objectT.Animator.SetTrigger("Revive");
        //objectT.ThingHappen(new() { SoundName = "Melee" });
        Debug.Log("enter Revive");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT.ObjectHealth.IsRevive) return;
        if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
        //if (StateTransition.TransitionHealthNegative(objectT)) return;
        //if (StateTransition.TransitionDueAction(objectT)) return;
        StateTransitionCharacter.TransitionSpeed(objectT);
    }

    public override void Exit(ObjectT objectT)
    {
        base.Exit(objectT);
        if (objectT is Character character)
        {
            character.CharacterMovement.StartMoveDirection();
        }
    }
}
