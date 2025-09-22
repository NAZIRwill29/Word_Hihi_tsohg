public interface ICommandExecuteUndoable
{
    public void ExecuteUndoable(string name = "normal");
    public void Undo(string name = "normal");
    public void Redo(string name = "normal");
}