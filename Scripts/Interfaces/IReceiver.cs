public interface IReceiver
{
    public string Name { get; }
    public ObjectStatsManager ObjectStatsManager { get; }
    public ObjectT ObjectT { get; }
}
