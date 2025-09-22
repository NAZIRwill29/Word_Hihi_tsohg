/// <summary>
/// Defines a contract for switchable objects. This interface helps implement the Dependency Inversion Principle (DIP)
/// by abstracting the details of activating and deactivating objects.
/// </summary>
public interface ISwitchable
{
    public bool IsActive { get; }

    public void Activate();
    public void Deactivate();
}