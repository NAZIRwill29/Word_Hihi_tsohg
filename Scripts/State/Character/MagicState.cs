using UnityEngine;

[CreateAssetMenu(fileName = "MagicState", menuName = "State/CharacterStates/MagicState")]
public class MagicState : AbilityState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            character.ObjectMagic.IsMagic = false;
            objectT.Animator.SetTrigger("Magic");
            string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
            objectT.ThingHappen(new() { SoundName = soundName });
            Debug.Log("enter Magic");
        }
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectMagic.IsMagic) return;
            if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
            if (StateTransitionCharacter.TransitionHealthNegative(objectT)) return;
        }
        base.Execute(objectT);
    }
}
