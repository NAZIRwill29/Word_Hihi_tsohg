using UnityEngine;

[CreateAssetMenu(fileName = "HitState", menuName = "State/CharacterStates/HitState")]
public class HitState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        objectT.ObjectHealth.IsHit = false;
        objectT.Animator.SetTrigger("Hit");
        //objectT.ThingHappen(new() { SoundName = "Melee" });
        Debug.Log("enter Hit");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT.ObjectHealth.IsHit)
        {
            Debug.Log("IsHit");
            return;
        }
        //if (StateTransition.TransitionHealthPositive(objectT)) return;
        if (StateTransitionCharacter.TransitionHealthNegative(objectT))
        {
            Debug.Log("TransitionHealthNegative");
            return;
        }
        //if (StateTransition.TransitionDueAction(objectT)) return;
        StateTransitionCharacter.TransitionSpeed(objectT);
    }
}
