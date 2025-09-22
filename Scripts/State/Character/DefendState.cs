using UnityEngine;

[CreateAssetMenu(fileName = "DefendState", menuName = "State/CharacterStates/DefendState")]
public class DefendState : AbilityState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            character.ObjectDefense.IsDefend = false;
            objectT.Animator.SetTrigger("Defend");
            string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
            objectT.ThingHappen(new() { SoundName = soundName });
            Debug.Log("enter Defend");
        }
    }

    public override void Execute(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectDefense.IsDefend) return;
            if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
        }
        base.Execute(objectT);
    }
}
