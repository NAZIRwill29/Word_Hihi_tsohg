using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMagic : ObjectStat, IMagic
{
    [SerializeField] protected ActionManager m_ActionManager;
    public bool IsMagic { get; set; }
    public bool IsInvulnerable;
    public bool IsHit { get; set; }
    public bool IsHeal { get; set; }
    [SerializeField] private MagicData m_tempMagicData;
    [SerializeField] protected ThingHappenTriggerSO m_HealThingHappenTriggerSO, m_DamageThingHappenTriggerSO, m_InvulnerableThingHappenTriggerSO;
    public UnityEvent<StatsMicrobarData> ManaChanged;
    public UnityEvent<float> OnRecoveryPlus;
    public UnityEvent<bool> OnRecoverySpeedInc;
    private ThingHappenData m_HealRuntimeThingHappenData = new();
    private ThingHappenData m_DamageRuntimeThingHappenData = new();
    private ThingHappenData m_InvulnerableRuntimeThingHappenData = new();

    //public event UnityAction<string, Projectile> OnMagic;
    public void Magic(string name = "normal", float cost = 0, ExecuteActionCommandData data = null)
    {
        //TODO in next proj() - LIKE PROJECTILE
        //do magic
        Debug.Log("do magic");
        IsMagic = true;

        VariableChanger.ChangeNameVariable(StatsData.DataNumVars, "Mana",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.IsIncrement = true;
                target.AddNumVariable = -cost;
            }
        );
        ChangeManaVariable(m_tempMagicData, false);

        // ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
        //     this,
        //     StatsData.DataNumVars,
        //     "Magic",
        //     dataNumVar => dataNumVar.NumVariable--
        // );
        // if (data.Poolable is Projectile projectile)
        //OnMagic?.Invoke(name, projectile);
    }

    public void ChangeRecoveryMana(float num, float duration, bool isIncrease, bool isPermanent = false)
    {
        ChangeRecoveryStat("Mana", num, duration, isPermanent);
        OnRecoverySpeedInc.Invoke(isIncrease);
    }

    public override void ChangeAllVariable(object data, bool inZone)
    {
        base.ChangeAllVariable(data, inZone);
        //Debug.Log("health() - ChangeAllVariable " + GetTime.GetCurrentTime("full-ms"));
        if (data is MagicData magicData)
            ChangeManaVariable(magicData);
    }

    protected override void ChangeAllVariable(object data)
    {
        base.ChangeAllVariable(data);
        if (data is MagicData magicData)
            ChangeManaVariable(magicData);
    }

    void ChangeManaVariable(MagicData data, bool isObjecTThingHappen = true)
    {
        if (data == null || StatsData == null) return;

        StatsData.EffectorName = data.OwnerName;

        VariableChanger.ChangeNameVariable(StatsData.DataNumVars, "Mana",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                // Fetch source variable from data

                DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(data.DataNumVars, "Mana");
                //DataNumericalVariable source = data.DataNumVars.FirstOrDefault(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase));

                // If source is null, exit early
                if (source == null)
                    return;

                // Get index of target variable, can be used for logging if necessary
                int id = data.DataNumVars.FindIndex(v => v.Name.Equals("Mana", StringComparison.OrdinalIgnoreCase));

                if (target.Cooldown <= 0)
                {
                    // Update target variable based on source
                    target.IsCanRecovery = source.IsCanRecovery != BoolNull.IsNull ? source.IsCanRecovery : target.IsCanRecovery;
                    //Debug.Log("AddNumVariable 3 " + source.AddNumVariable);
                    target.AddNumVariable = source.AddNumVariable;
                    target.NumVariable = source.IsIncrement
                        ? target.NumVariable + target.AddNumVariable
                        : target.NumVariableOri + target.AddNumVariable;
                    //Debug.Log("SChangeNumVariable " + target.Name + " " + target.NumVariable + " : add " + target.AddNumVariable);

                    target.AddNumVariableMax = source.AddNumVariableMax;
                    target.NumVariableMax = source.IsIncrement
                        ? target.NumVariableMax + target.AddNumVariableMax
                        : target.NumVariableMaxOri + target.AddNumVariableMax;

                    target.NumVariableOri = source.IsIncrement ? target.NumVariable : target.NumVariableOri;
                    target.NumVariableMaxOri = source.IsIncrement ? target.NumVariableMax : target.NumVariableMaxOri;

                    target.TimeCoolDown = source.TimeCoolDown > 0 ? source.TimeCoolDown : target.TimeCoolDown;
                    target.Cooldown = source.TimeCoolDown;

                    // Handle poison logic
                    if (source.IsPoison && !target.IsPoisonImmune)
                    {
                        target.IsPoison = true;
                        target.PoisonTimeCoolDown = source.PoisonTimeCoolDown > 0 ?
                            source.PoisonTimeCoolDown : target.PoisonTimeCoolDown;

                        target.PoisonCooldown = Mathf.Max(target.PoisonCooldown, target.PoisonTimeCoolDown);
                        target.PoisonDuration = source.PoisonDuration > 0 ?
                            source.PoisonDuration : target.PoisonDuration;

                        target.PoisonAmount = source.PoisonAmount;
                    }

                    // Create HealthData and trigger the event
                    MagicData magicData = new()
                    {
                        DataNumVars = new List<DataNumericalVariable> { target },
                        MicrobarAnimType = data.MicrobarAnimType,
                        EffectorName = data.OwnerName
                    };

                    if (isObjecTThingHappen)
                    {
                        if (source.AddNumVariable > 0)
                        {
                            m_HealThingHappenTriggerSO.ObjecTThingHappen(
                                m_ObjectT,
                                m_HealRuntimeThingHappenData,
                                " " + Mathf.Abs(source.AddNumVariable)
                            );
                        }
                        else
                        {
                            m_DamageThingHappenTriggerSO.ObjecTThingHappen(
                                m_ObjectT,
                                m_DamageRuntimeThingHappenData,
                                " " + Mathf.Abs(source.AddNumVariable)
                            );
                        }
                    }
                    OnRecoveryPlus.Invoke(source.AddNumVariable);

                    ManaChanged.Invoke(magicData);
                }
            }
        );
    }

    public void TakeDamage(float amount)
    {
        // Validate the amount
        if (amount <= 0)
            return;

        VariableChanger.ChangeNameVariable(m_tempMagicData.DataNumVars, "Mana",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.IsIncrement = true;
                target.AddNumVariable = -amount;
            }
        );
        TakeDamage(m_tempMagicData);
    }

    public void Heal(float amount)
    {
        // Validate the amount
        if (amount <= 0)
            return;

        VariableChanger.ChangeNameVariable(m_tempMagicData.DataNumVars, "Mana",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.IsIncrement = true;
                target.AddNumVariable = amount;
            }
        );
        Heal(m_tempMagicData);
    }

    public void Heal(MagicData magicData)
    {
        if (!m_ObjectT.IsAlive) return;

        IsHeal = true;
        ChangeAllVariable(magicData);
    }

    public void TakeDamage(MagicData magicData)
    {
        if (!m_ObjectT.IsAlive) return;
        if (IsInvulnerable)
        {
            m_InvulnerableThingHappenTriggerSO.ObjecTThingHappen(m_ObjectT, m_InvulnerableRuntimeThingHappenData, string.Empty);
            return;
        }

        IsHit = true;
        ChangeAllVariable(magicData);
        //CheckMana();
    }

    public override void PoisonHappened(ListPoisonData listPoisonData)
    {
        base.PoisonHappened(listPoisonData);
        //CheckMana();
    }

    // void CheckMana()
    // {
    //     //if (StatsData.DataNumVars.FirstOrDefault(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase)).NumVariable <= 0)
    //     if (VariableFinder.GetVariableContainNameFromList(StatsData.DataNumVars, "Mana").NumVariable <= 0)
    //         Die();
    // }
}
