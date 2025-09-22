using System.Collections.Generic;

public class ActionCommandPool
{
    private static Queue<ActionCommand> pool = new();

    // Pre-populate the pool with a fixed number of objects
    static ActionCommandPool()
    {
        for (int i = 0; i < 10; i++) // Adjust size based on expected usage
        {
            pool.Enqueue(new ActionCommand(null, 0, "", "", 0, 0, null));
        }
    }

    public static ActionCommand GetCommand(ICommandExecuteDelay commandExecuteDelay, int objectId, string name, string idName, float delay, float cooldown, ExecuteActionCommandData data)
    {
        if (pool.Count > 0)
        {
            ActionCommand cmd = pool.Dequeue();
            cmd.Initialize(commandExecuteDelay, objectId, name, idName, delay, cooldown, data);
            return cmd;
        }

        // Instead of creating a new object, return a recycled one
        ActionCommand recycledCmd = new ActionCommand(null, 0, "", "", 0, 0, null);
        recycledCmd.Initialize(commandExecuteDelay, objectId, name, idName, delay, cooldown, data);
        return recycledCmd;
    }

    public static void ReturnCommand(ActionCommand cmd)
    {
        cmd.Reset(); // Reset command before reuse
        pool.Enqueue(cmd);
    }
}
