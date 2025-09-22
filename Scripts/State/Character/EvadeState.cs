using UnityEngine;

[CreateAssetMenu(fileName = "EvadeState", menuName = "State/CharacterStates/EvadeState")]
public class EvadeState : AbilityState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            character.ObjectEvadeable.IsEvade = false;
            objectT.Animator.SetTrigger("Evade");
            string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
            objectT.ThingHappen(new() { SoundName = soundName });
            Debug.Log("enter Evade");
        }
    }

    public override void Execute(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectEvadeable.IsEvade) return;
            if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
        }
        base.Execute(objectT);
    }
}
