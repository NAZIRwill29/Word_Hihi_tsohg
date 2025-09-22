using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkState", menuName = "State/CharacterStates/WalkState")]
public class WalkState : ScriptableState
{
    [SerializeField] private NameDataFlyweight m_SoundNameDataFlyweight;
    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        // // if we are no longer grounded, transition to jumping
        // if (!player.IsGrounded)
        // {
        //     player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
        // }
        if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
        if (StateTransitionCharacter.TransitionHealthNegative(objectT)) return;
        if (StateTransitionCharacter.TransitionDueAction(objectT)) return;
        StateTransitionCharacter.TransitionSpeed(objectT);
    }
    public override void Exit(ObjectT objectT)
    {
        base.Exit(objectT);
        string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
        objectT.ObjectAudioMulti.StopPlaySound(soundName);
    }
}