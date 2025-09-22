using UnityEngine;

public class Receiver : MonoBehaviour, IReceiver
{
    [SerializeField] private NameDataFlyweight m_NameDataFlyweight;
    public string Name { get => m_NameDataFlyweight.Name; }
    public ObjectStatsManager ObjectStatsManager { get; set; }
    public ObjectT ObjectT { get; set; }

    protected virtual void Awake()
    {
        ObjectT = GetComponentInParent<ObjectT>();
        ObjectStatsManager = GetComponentInParent<ObjectStatsManager>();
    }
}
