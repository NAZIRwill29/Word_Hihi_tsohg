using UnityEngine;

public class AbilityState : ScriptableState
{
    [SerializeField] protected NameDataFlyweight m_SoundNameDataFlyweight;
    public override void Execute(ObjectT objectT)
    {
        //if (StateTransition.TransitionDueAction(objectT)) return;
        StateTransitionCharacter.TransitionSpeed(objectT);
    }
}
