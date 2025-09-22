using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// CommandList class to handle a queue of commands and execution status
public class CommandDelayList
{
    // public List<ICommandDelay> s_ExecutedCommands = new List<ICommandDelay>();
    // public int currentCommandIndex = -1;
    public Queue<ICommandDelay> s_CommandQueue = new Queue<ICommandDelay>();
    public bool isExecuting = false;
}

// CommandQueueInvoker class
public class CommandQueueInvoker : Singleton<CommandQueueInvoker>
{
    // Command dictionary to manage command queues by ObjectId and IdName
    private Dictionary<string, Dictionary<string, CommandDelayList>> m_CommandObjectDict = new();

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (m_CommandObjectDict.Count == 0) return;

        foreach (var m_CommandDict in m_CommandObjectDict.Values)
        {
            if (m_CommandDict == null || m_CommandDict.Count == 0) continue;

            // Iterate over the dictionary and process commands if not already executing
            foreach (var item in m_CommandDict)
            {
                CommandDelayList commandList = item.Value;
                if (commandList == null) continue;

                if (!commandList.isExecuting && commandList.s_CommandQueue.Count > 0)
                {
                    StartCoroutine(ProcessCommands(commandList));
                }
            }
        }
    }

    //ABILITY() - 3
    // Method to execute a command
    public void ExecuteCommand(ICommandDelay command, int maxSize)
    {
        if (command == null)
        {
            Debug.LogWarning("Command is null. Skipping execution.");
            return;
        }

        string ObjectIdString = command.ObjectId.ToString();

        // Optimize object allocation by reusing existing dictionary
        if (!m_CommandObjectDict.TryGetValue(ObjectIdString, out var commandDict))
        {
            commandDict = new Dictionary<string, CommandDelayList>();
            m_CommandObjectDict[ObjectIdString] = commandDict;
        }

        if (!commandDict.TryGetValue(command.IdName, out var commandList))
        {
            commandList = new CommandDelayList();
            commandDict[command.IdName] = commandList;
        }

        commandList.s_CommandQueue.Enqueue(command);

        // Enforce max size of command queue
        if (commandList.s_CommandQueue.Count > maxSize)
        {
            commandList.s_CommandQueue.Dequeue();
        }
    }

    //ABILITY() - 4
    // Process commands for a specific CommandList
    private IEnumerator ProcessCommands(CommandDelayList commandList)
    {
        if (commandList == null)
        {
            Debug.LogWarning("CommandList is null. Skipping processing.");
            yield break;
        }

        commandList.isExecuting = true;

        while (commandList.s_CommandQueue.Count > 0)
        {
            // Use Peek() instead of First() to get the first element
            ICommandDelay currentCommand = commandList.s_CommandQueue.Peek();

            if (currentCommand == null)
            {
                Debug.LogWarning("Command in queue is null. Skipping.");
                commandList.s_CommandQueue.Dequeue(); // Remove null command to prevent an infinite loop
                continue;
            }

            // Start executing the command
            StartCoroutine(currentCommand.ExecuteDelay());

            // Wait for cooldown before dequeuing the command
            yield return new WaitForSeconds(currentCommand.Cooldown);
            commandList.s_CommandQueue.Dequeue(); // Now remove the processed command
        }

        commandList.isExecuting = false;
    }
}
