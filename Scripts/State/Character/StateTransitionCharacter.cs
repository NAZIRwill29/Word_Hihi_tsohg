using UnityEngine;

//TODO() - create state for item - weapon, projectile
public static class StateTransitionCharacter
{
    public static void TransitionSpeed(ObjectT objectT)
    {
        if (objectT is Character character)
        {

            if (character.ObjectSpeed != null && character.ObjectSpeed.IsStandBy)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["StandBy"]);
                return;
            }
            // if we slow to within a minimum velocity, transition to idling/standing
            else if (Mathf.Abs(character.CharacterMovement.CurrentSpeed) < 0.1f)
            {
                if (character.ObjectStateView.CurrentState == character.StateMachineScriptable.StateDict["Idle"])
                {
                    character.ThingHappen(new() { SoundName = "Idle" });
                    return;
                }
                character.StateMachineScriptable.TransitionTo(character, character.StateMachineScriptable.StateDict["Idle"]);
            }
            else
            {
                if (character.ObjectStateView.CurrentState == character.StateMachineScriptable.StateDict["Walk"])
                {
                    character.ThingHappen(new() { SoundName = "Walk" });
                    return;
                }
                character.StateMachineScriptable.TransitionTo(character, character.StateMachineScriptable.StateDict["Walk"]);
            }
        }
    }

    public static bool TransitionHealthPositive(ObjectT objectT)
    {
        if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsHeal)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Heal"]);
            return true;
        }
        if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsRevive)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Revive"]);
            return true;
        }
        return false;
    }

    public static bool TransitionHealthNegative(ObjectT objectT)
    {
        if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsHit)
        {
            Debug.Log("hit");
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Hit"]);
            return true;
        }
        if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsDie)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Die"]);
            return true;
        }
        return false;
    }

    public static bool TransitionDueAction(ObjectT objectT)
    {
        if (objectT is Character character)
        {
            if (character.ObjectShoot != null && character.ObjectShoot.IsShoot)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Shoot"]);
                return true;
            }
            if (character.ObjectDefense != null && character.ObjectDefense.IsDefend)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Defend"]);
                return true;
            }
            if (character.ObjectEvadeable != null && character.ObjectEvadeable.IsEvade)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Evade"]);
                return true;
            }
            if (character.ObjectMagic != null && character.ObjectMagic.IsMagic)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Magic"]);
                return true;
            }
            if (character.ObjectMelee != null && character.ObjectMelee.IsMelee)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Melee"]);
                return true;
            }
            if (character.ObjectMelee != null && character.ObjectMelee.IsGoNear)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["GoNear"]);
                return true;
            }
        }
        return false;
    }
}
