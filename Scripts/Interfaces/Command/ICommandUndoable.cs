// interface to wrap your actions in a "command object"
public interface ICommandUndoable : ICommand
{
    public void ExecuteUndoable();
    public ICommandExecuteUndoable ICommandExecuteUndoable { get; set; }
    public void Undo();
}