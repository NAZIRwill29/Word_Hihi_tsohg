using UnityEngine;

[CreateAssetMenu(fileName = "StandByState", menuName = "State/CharacterStates/StandByState")]
public class StandByState : ScriptableState
{
    public override void Enter(ObjectT objectT)
    {
        //if (objectT is Character character)
        //{
        //character.ObjectSpeed.IsStandBy = false;
        objectT.Animator.SetTrigger("StandBy");
        Debug.Log("enter StandBy");
        //}
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectSpeed.IsStandBy) return;
            if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
            if (StateTransitionCharacter.TransitionHealthNegative(objectT)) return;
        }
        base.Execute(objectT);
    }
}
