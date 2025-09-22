using UnityEngine;

[CreateAssetMenu(fileName = "GoNearState", menuName = "State/CharacterStates/GoNearState")]
public class GoNearState : MeleeState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            character.ObjectMelee.IsGoNear = false;
            objectT.Animator.SetTrigger("GoNear");
            string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
            objectT.ThingHappen(new() { SoundName = soundName });
            Debug.Log("enter GoNear");
        }
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectMelee.IsGoNear) return;
            if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
            if (StateTransitionCharacter.TransitionHealthNegative(objectT)) return;
        }
        base.Execute(objectT);
    }
}
