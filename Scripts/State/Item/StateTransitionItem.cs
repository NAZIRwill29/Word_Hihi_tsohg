using UnityEngine;

public static class StateTransitionItem
{
    public static void TransitionSpeed(ObjectT objectT)
    {
        if (objectT is ItemObjectT itemObjectT)
        {
            // if we slow to within a minimum velocity, transition to idling/standing
            if (!itemObjectT.IsMoveState)
            {
                if (itemObjectT.ObjectStateView.CurrentState == itemObjectT.StateMachineScriptable.StateDict["Idle"])
                {
                    itemObjectT.ThingHappen(new() { SoundName = "Idle" });
                    return;
                }
                itemObjectT.StateMachineScriptable.TransitionTo(itemObjectT, itemObjectT.StateMachineScriptable.StateDict["Idle"]);
            }
            else
            {
                if (itemObjectT.ObjectStateView.CurrentState == itemObjectT.StateMachineScriptable.StateDict["Move"])
                {
                    itemObjectT.ThingHappen(new() { SoundName = "Move" });
                    return;
                }
                itemObjectT.StateMachineScriptable.TransitionTo(itemObjectT, itemObjectT.StateMachineScriptable.StateDict["Move"]);
            }
        }
    }

    public static bool TransitionHealthPositive(ObjectT objectT)
    {
        // if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsHeal)
        // {
        //     objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Heal"]);
        //     return true;
        // }
        if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsRevive)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Revive"]);
            return true;
        }
        return false;
    }

    public static bool TransitionHealthNegative(ObjectT objectT)
    {
        // if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsHit)
        // {
        //     objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Hit"]);
        //     return true;
        // }
        if (objectT.ObjectHealth != null && objectT.ObjectHealth.IsDie)
        {
            objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Destroy"]);
            return true;
        }
        return false;
    }

    public static bool TransitionActive(ObjectT objectT)
    {
        if (objectT is ItemObjectT itemObjectT)
        {
            if (itemObjectT.IsActiveState)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["Active"]);
                return true;
            }
            if (itemObjectT.IsInActiveState)
            {
                objectT.StateMachineScriptable.TransitionTo(objectT, objectT.StateMachineScriptable.StateDict["InActive"]);
                //itemObjectT.Deactivate();
                return true;
            }
        }
        return false;
    }
}
