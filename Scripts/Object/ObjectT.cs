using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ThingHappenData
{
    public string SoundName;
    public string FXName;
    public TextFloatData TextFloatData = new();
}

[System.Serializable]
public class TextFloatData
{
    public string Msg;
    public int FontSize = 20;
    public Color Color = Color.green;
    public Vector3 Position;
    public Vector3 Motion = Vector3.up * 25;
    public float Duration = 1.5f;
}
//
public class ObjectT : MonoBehaviour, IObjectStatProvider
{
    public Transform ObjTransform { set; get; }
    //use for FXName - Player/Enemy/NPC
    public NameDataFlyweight ObjectTTypeNameData;
    public NameDataFlyweight ObjectTNameNameData;
    [SerializeField] protected StateMachineScriptable stateMachineScriptable;
    public StateMachineScriptable StateMachineScriptable => stateMachineScriptable;
    public int ObjectId;
    public Animator Animator;
    public ObjectStateView ObjectStateView;
    public ObjectAudioMulti ObjectAudioMulti;
    public ObjectFXMulti ObjectFXMulti;
    public ObjectHealth ObjectHealth;
    [ReadOnly] public bool IsAlive = true;
    [ReadOnly] public bool IsStabilityAlive = true;
    [ReadOnly] public bool InBattle = false;
    //use when thing happen
    public event Action<ThingHappenData> OnThingHappened;
    public GameObject Center, CenterTransf;
    public ObjectStatsManager ObjectStatsManager;
    public bool IsCanExecute = true;

    protected virtual void Awake()
    {
        if (stateMachineScriptable == null)
        {
            Debug.LogWarning("ObjectT: StateMachineScriptable is not assigned in the Inspector!");
        }
        ObjectId = GetInstanceID();
    }

    protected virtual void Start()
    {
        if (stateMachineScriptable != null)
        {
            stateMachineScriptable.Initialize(this);
        }
        else
        {
            Debug.LogWarning("ObjectT: No states assigned to the StateMachineScriptable!");
        }
        ObjTransform = GetComponent<Transform>();
    }

    // Called via Unity Event in the Inspector
    protected virtual void Update()
    {
        if (!IsCanExecute) return;
        //Debug.Log("Update");
        if (stateMachineScriptable != null)
        {
            stateMachineScriptable.Execute(this);
        }
    }

    public ObjectStat GetObjectStatByName(string name)
    {
        return ObjectStatsManager.GetObjectStatByName(name);
    }

    public virtual void ThingHappen(ThingHappenData thingHappenData)
    {
        //Debug.Log("ThingHappen");
        OnThingHappened?.Invoke(thingHappenData);
        if (thingHappenData.TextFloatData != null)
            GameManager.Instance.FloatingTextManager.Show(
                thingHappenData.TextFloatData.Msg,
                thingHappenData.TextFloatData.FontSize,
                thingHappenData.TextFloatData.Color,
                thingHappenData.TextFloatData.Position,
                thingHappenData.TextFloatData.Motion,
                thingHappenData.TextFloatData.Duration
            );
    }

    public virtual void RefreshNatureElement()
    {
    }
}
