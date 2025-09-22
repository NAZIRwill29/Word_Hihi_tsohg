using System.Collections;
using UnityEngine;

// An example of a simple command object, implementing ICommand
public class LearnAbilityCommand : ISingleCommandUndoable
{
    public int ObjectId { get; set; }
    public string IdName { get; set; } = "learnAbilty";
    public ICommandExecuteUndoable ICommandExecuteUndoable { get; set; }
    public string CmdName { get; set; }

    // Constructor
    public LearnAbilityCommand(ICommandExecuteUndoable commandExecute, int objectId, string name, string idName = "learnAbilty")
    {
        ICommandExecuteUndoable = commandExecute;
        CmdName = name;
        IdName = idName;
        ObjectId = objectId;
    }
    public void ExecuteUndoable()
    {
        ICommandExecuteUndoable.ExecuteUndoable(CmdName);
    }
    // Logic of thing to do goes here
    public void Execute()
    {

    }
    // Undo logic goes here
    public void Undo()
    {
        ICommandExecuteUndoable.Undo(CmdName);
    }
}