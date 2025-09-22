using UnityEngine;

[CreateAssetMenu(fileName = "MeleeState", menuName = "State/CharacterStates/MeleeState")]
public class MeleeState : AbilityState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            character.ObjectMelee.IsMelee = false;
            objectT.Animator.SetTrigger("Melee");
            string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
            objectT.ThingHappen(new() { SoundName = soundName });
            Debug.Log("enter Melee");
        }
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectMelee.IsMelee) return;
            if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
            if (StateTransitionCharacter.TransitionHealthNegative(objectT)) return;
        }
        base.Execute(objectT);
    }
}
