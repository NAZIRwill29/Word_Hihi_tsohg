using UnityEngine;

public class ExecuteActionCommandData
{
    public IPoolable Poolable { get; set; }
    // public int SomeOtherProperty { get; set; } // Example property

    public void Reset()
    {
        Poolable = null; // Release reference
    }
}

// An example of a simple command object, implementing ICommand
[CreateAssetMenu(fileName = "ActionManager", menuName = "Manager/ActionManager")]
public class ActionManager : ScriptableObject
{
    //ABILITY() - 2
    public void ExecuteActionCommand(ICommandExecuteDelay commandExecuteDelay, int objectId, string name = "normal", string idName = "action", float delay = 0f, float cooldown = 0f, int maxSize = 50, ExecuteActionCommandData data = null)
    {
        if (IsValidMove(name))
        {
            //Debug.Log("(3)ExecuteActionCommand " + GetTime.GetCurrentTime("full-ms"));
            // Create a new AttackCommand with the provided delay 
            ActionCommand abilityCommand = ActionCommandPool.GetCommand(commandExecuteDelay, objectId, name, idName, delay, cooldown, data);
            ICommandDelay command = abilityCommand;

            // Execute the command immediately and save it to the stack
            CommandQueueInvoker.Instance.ExecuteCommand(command, maxSize);
        }
    }

    public bool IsValidMove(string name)
    {
        // Add your movement validation logic here
        return true;
    }
}