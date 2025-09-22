using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class CommandUndoableList
{
    // Stack of command objects to undo
    public Stack<ICommandUndoable> s_UndoStack = new Stack<ICommandUndoable>();
    // Stack of redoable commands
    public Stack<ICommandUndoable> s_RedoStack = new Stack<ICommandUndoable>();
}

public class CommandInvoker
{
    // Dictionary to store undoable and redoable commands for each ID
    private static Dictionary<string, Dictionary<string, CommandUndoableList>> s_CommandObjectDict = new();

    // Execute a command object directly and save it to the undo stack
    public static void ExecuteCommand(ICommandUndoable command)
    {
        if (command == null)
        {
            Debug.LogWarning("Command is null. Skipping execution.");
            return;
        }
        string ObjectIdString = command.ObjectId.ToString();

        // Execute the command
        command.ExecuteUndoable();

        // Ensure the command list exists for the given ID
        if (!s_CommandObjectDict.ContainsKey(ObjectIdString))
        {
            s_CommandObjectDict[ObjectIdString] = new();
        }
        if (!s_CommandObjectDict[ObjectIdString].ContainsKey(command.IdName))
        {
            s_CommandObjectDict[ObjectIdString][command.IdName] = new CommandUndoableList();
        }

        // Check if the command exists in the undo stack without using LINQ
        bool commandExists = false;
        foreach (var existingCommand in s_CommandObjectDict[ObjectIdString][command.IdName].s_UndoStack)
        {
            if (existingCommand.CmdName == command.CmdName)
            {
                commandExists = true;
                break;
            }
        }

        if (command is not ISingleCommandUndoable || !commandExists)
        {
            // Add the command to the undo stack
            s_CommandObjectDict[ObjectIdString][command.IdName].s_UndoStack.Push(command);
        }

        // Clear the redo stack when a new command is executed
        s_CommandObjectDict[ObjectIdString][command.IdName].s_RedoStack.Clear();
        Debug.Log($"Command executed and added to Undo stack: {command.IdName}");
    }

    // Undo the last command
    public static void UndoCommand(string objectId, string idName)
    {
        if (!s_CommandObjectDict.ContainsKey(objectId))
        {
            Debug.LogWarning($"No commands found for objectId: {objectId}. Cannot undo.");
            return;
        }

        if (!s_CommandObjectDict[objectId].ContainsKey(idName))
        {
            Debug.LogWarning($"No commands found for IdName: {idName}. Cannot undo.");
            return;
        }

        CommandUndoableList commandList = s_CommandObjectDict[objectId][idName];
        if (commandList.s_UndoStack.Count == 0)
        {
            Debug.LogWarning($"No commands to undo for IdName: {idName}.");
            return;
        }

        // Undo the last command
        ICommandUndoable activeCommand = commandList.s_UndoStack.Pop();
        commandList.s_RedoStack.Push(activeCommand);
        activeCommand.Undo();
        Debug.Log($"Undo executed for command: {idName}");
    }

    // Redo the last undone command
    public static void RedoCommand(string objectId, string idName)
    {
        if (!s_CommandObjectDict.ContainsKey(objectId) || !s_CommandObjectDict[objectId].ContainsKey(idName))
        {
            Debug.LogWarning($"No commands found for IdName: {idName}. Cannot redo.");
            return;
        }

        CommandUndoableList commandList = s_CommandObjectDict[objectId][idName];
        if (commandList.s_RedoStack.Count == 0)
        {
            Debug.LogWarning($"No commands to redo for IdName: {idName}.");
            return;
        }

        // Redo the last undone command
        ICommandUndoable activeCommand = commandList.s_RedoStack.Pop();
        commandList.s_UndoStack.Push(activeCommand);
        activeCommand.ExecuteUndoable();
        Debug.Log($"Redo executed for command: {idName}");
    }

    public static void LogDictionaryContents(string logFilePath)
    {
        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath) ?? ".");

        using (StreamWriter writer = new StreamWriter(logFilePath))
        {
            foreach (var objectPair in s_CommandObjectDict)
            {
                writer.WriteLine($"Object ID: {objectPair.Key}");

                foreach (var idPair in objectPair.Value)
                {
                    writer.WriteLine($"  IdName: {idPair.Key}");

                    writer.WriteLine("    Undo Stack:");
                    foreach (var command in idPair.Value.s_UndoStack)
                    {
                        writer.WriteLine($"      {command.CmdName}");
                    }

                    writer.WriteLine("    Redo Stack:");
                    foreach (var command in idPair.Value.s_RedoStack)
                    {
                        writer.WriteLine($"      {command.CmdName}");
                    }

                    writer.WriteLine(); // Separate entries with a blank line
                }
            }
        }

        Debug.Log($"dictionary in command invoker written to: {logFilePath}");
    }
}
