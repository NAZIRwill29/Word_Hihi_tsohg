using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpState", menuName = "State/CharacterStates/JumpState")]
public class JumpState : AbilityState
{
    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {

        //Debug.Log("Updating Jump State");

        // if (player.IsGrounded)
        // {
        //     if (Mathf.Abs(player.CharController.velocity.x) > 0.1f || Mathf.Abs(player.CharController.velocity.z) > 0.1f)
        //     {
        //         player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
        //     }
        //     else
        //     {
        //         player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
        //     }
        // }
    }
}