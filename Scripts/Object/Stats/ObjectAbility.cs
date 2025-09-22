using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectAbility : ObjectStat, IAbility, ICommandExecuteDelay
{
    [SerializeField] protected EquipAbilityObject m_EquipAbilityObject;
    [SerializeField] protected ActionManager m_ActionManager;
    protected ExecuteActionCommandData m_ExecuteActionCommandData = new();

    protected override void Start()
    {
        base.Start();

        if (m_EquipAbilityObject == null)
        {
            Debug.LogError("EquipAbilityObject is not assigned in ObjectAbility!");
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private Ability GetAbility(string abilityName)
    {
        foreach (var ability in m_EquipAbilityObject.EquipAbilities.Items)
        {
            if (ability.AbilityName == abilityName)
                return ability;
        }
        return null;
    }

    public void DoAbilityA(CommandDataFlyweight commandDataFlyweight)
    {
        DoAbility(commandDataFlyweight);
    }

    //ABILITY() - 1
    public void DoAbility(CommandDataFlyweight commandDataFlyweight, int maxSize = 50, string pooledObjName = null)
    {
        if (m_EquipAbilityObject == null)
        {
            Debug.LogWarning("EquipAbilityObject is null. Cannot perform ability.");
            return;
        }

        Ability ability = GetAbility(commandDataFlyweight.CmdName);
        if (ability == null)
        {
            Debug.LogWarning($"Ability '{commandDataFlyweight.CmdName}' not available in EquipAbilityObject.");
            return;
        }

        if (m_ActionManager != null)
        {
            if (string.IsNullOrEmpty(pooledObjName))
            {
                m_ActionManager.ExecuteActionCommand(this, m_ObjectT.ObjectId, commandDataFlyweight.CmdName, commandDataFlyweight.IdName, ability.Delay, ability.Cooldown, maxSize);
            }
            else
            {
                m_ExecuteActionCommandData.Reset();
                if (GameManager.Instance.ObjectPool.GetPooledObject(pooledObjName) is IPoolable pooledObject)
                {
                    m_ExecuteActionCommandData.Poolable = pooledObject;
                    m_ActionManager.ExecuteActionCommand(this, m_ObjectT.ObjectId, commandDataFlyweight.CmdName, commandDataFlyweight.IdName, ability.Delay, ability.Cooldown, maxSize, m_ExecuteActionCommandData);
                }
                else
                {
                    Debug.LogWarning($"{pooledObjName} not found in ObjectPool.");
                }
            }
        }
        else
        {
            Debug.LogWarning("ActionManager.Instance is null. Cannot execute action.");
        }
    }

    //ABILITY() - 6
    public void ExecuteDelay(string name = "normal", ExecuteActionCommandData data = null)
    {
        if (m_EquipAbilityObject == null)
        {
            Debug.LogWarning("EquipAbilityObject is null. Cannot execute ability.");
            return;
        }

        Ability ability = GetAbility(name);
        if (ability == null)
        {
            Debug.LogWarning($"Ability '{name}' not found in EquipAbilityObject.");
            return;
        }

        Debug.Log($"Executing Ability: {ability.AbilityName}");
        ability.Use(m_ObjectT, data);

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Ability",
            dataNumVar => dataNumVar.NumVariable--
        );
    }
}
