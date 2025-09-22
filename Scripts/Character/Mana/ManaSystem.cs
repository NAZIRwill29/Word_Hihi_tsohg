using UnityEngine;

public class ManaSystem : MircobarSystem
{
    [SerializeField] private ObjectMagic m_ObjectMagic;

    void OnEnable()
    {
        if (!m_ObjectMagic)
            return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectMagic, StatNumberNameData.Name).AddListener(NotifyStatChanged);
        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectMagic, StatBoolNameData.Name).AddListener(NotifyAliveChanged);
        m_ObjectMagic.ManaChanged.AddListener(NotifyStatChanged);
    }

    void OnDisable()
    {
        if (!m_ObjectMagic)
            return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectMagic, StatNumberNameData.Name).RemoveListener(NotifyStatChanged);
        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectMagic, StatBoolNameData.Name).RemoveListener(NotifyAliveChanged);
        m_ObjectMagic.ManaChanged.AddListener(NotifyStatChanged);
    }

    public void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        // if (objectStat is ObjectMagic objectMagic)
        //     m_ObjectMagic = objectMagic;
        // else
        //     return;
        if (m_ResetOnStart)
        {
            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectMagic,
                m_ObjectMagic.StatsData.DataNumVars,
                StatNumberNameData.Name,
                dataNumVar => dataNumVar.NumVariable = m_IsNumStartMaxVariable ? dataNumVar.NumVariableMax : dataNumVar.NumVariableMin
            );
        }
        float currentHealth = VariableFinder.GetVariableContainNameFromList(m_ObjectMagic.StatsData.DataNumVars, StatNumberNameData.Name).NumVariable;
        StatInit.Invoke(currentHealth);
    }
}

