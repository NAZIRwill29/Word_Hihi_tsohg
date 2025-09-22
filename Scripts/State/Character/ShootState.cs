using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootState", menuName = "State/CharacterStates/ShootState")]
public class ShootState : AbilityState
{
    public override void Enter(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            character.ObjectShoot.IsShoot = false;
            objectT.Animator.SetTrigger("Shoot");
            string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
            objectT.ThingHappen(new() { SoundName = soundName });
            Debug.Log("enter shoot");
        }
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        // // if we're no longer grounded, transition to jumping
        // if (!player.IsGrounded)
        // {
        //     player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
        // }

        //if we move above a minimum threshold, transition to walking
        if (objectT is Character character && character.ObjectShoot.IsShoot) return;
        if (StateTransitionCharacter.TransitionHealthPositive(objectT)) return;
        if (StateTransitionCharacter.TransitionHealthNegative(objectT)) return;
        base.Execute(objectT);
    }
}