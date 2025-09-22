using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectStability : ObjectStat, IStability, IDrain
{
    public bool IsInvulnerable;
    public bool IsHit { get; set; }
    public bool IsHeal { get; set; }
    public bool IsRevive { get; set; }
    public bool IsDie { get; set; }
    [SerializeField] private StabilityData m_tempStabilityData;
    [SerializeField] protected ThingHappenTriggerSO m_HealThingHappenTriggerSO, m_DamageThingHappenTriggerSO, m_InvulnerableThingHappenTriggerSO;
    [SerializeField] protected ThingHappenTriggerSO m_DieThingHappenTriggerSO, m_ReviveThingHappenTriggerSO;
    protected StabilityData m_ReviveStabilityData = new()
    {
        DataNumVars = new() { new() {
                Name = "Stability",
                IsIncrement = true,
            } },
        DataBoolVars = new() { new() {
                Name = "Alive",
                IsCan = BoolNull.IsTrue
            } }
    };
    public UnityEvent<StatsMicrobarData> StabilityChanged;
    private ThingHappenData m_HealRuntimeThingHappenData = new();
    private ThingHappenData m_DamageRuntimeThingHappenData = new();
    private ThingHappenData m_InvulnerableRuntimeThingHappenData = new();
    private ThingHappenData m_DieRuntimeThingHappenData = new();
    private ThingHappenData m_ReviveRuntimeThingHappenData = new();


    public override void ChangeAllVariable(object data, bool inZone)
    {
        base.ChangeAllVariable(data, inZone);
        //Debug.Log("health() - ChangeAllVariable " + GetTime.GetCurrentTime("full-ms"));
        if (data is StabilityData stabilityData)
            ChangeStabilityVariable(stabilityData);
    }

    protected override void ChangeAllVariable(object data)
    {
        base.ChangeAllVariable(data);
        if (data is StabilityData stabilityData)
            ChangeStabilityVariable(stabilityData);
    }

    protected void ChangeAllVariable(bool isSound, object data)
    {
        base.ChangeAllVariable(data);
        if (data is StabilityData stabilityData)
            ChangeStabilityVariable(stabilityData, isSound);
    }

    void ChangeStabilityVariable(StabilityData data, bool isSound = true)
    {
        if (data == null || StatsData == null) return;

        StatsData.EffectorName = data.OwnerName;

        VariableChanger.ChangeNameVariable(StatsData.DataNumVars, "Stability",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                // Fetch source variable from data

                DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(data.DataNumVars, "Stability");
                //DataNumericalVariable source = data.DataNumVars.FirstOrDefault(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase));

                // If source is null, exit early
                if (source == null)
                    return;

                // Get index of target variable, can be used for logging if necessary
                int id = data.DataNumVars.FindIndex(v => v.Name.Equals("Stability", StringComparison.OrdinalIgnoreCase));

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
                    StabilityData stabilityData = new()
                    {
                        DataNumVars = new List<DataNumericalVariable> { target },
                        MicrobarAnimType = data.MicrobarAnimType,
                        EffectorName = data.OwnerName
                    };

                    if (source.AddNumVariable > 0)
                    {
                        m_HealThingHappenTriggerSO.ObjecTThingHappen(
                            m_ObjectT,
                            m_HealRuntimeThingHappenData,
                            " " + Mathf.Abs(source.AddNumVariable),
                            isSound
                        );
                    }
                    else
                    {
                        m_DamageThingHappenTriggerSO.ObjecTThingHappen(
                            m_ObjectT,
                            m_DamageRuntimeThingHappenData,
                            " " + Mathf.Abs(source.AddNumVariable),
                            isSound
                        );
                    }

                    //Debug.Log("StabilityChanged Invoke");
                    StabilityChanged.Invoke(stabilityData);
                }
            }
        );
    }

    public void TakeDamage(int amount, bool isSound = true)
    {
        // Validate the amount
        if (amount <= 0)
            return;

        VariableChanger.ChangeNameVariable(m_tempStabilityData.DataNumVars, "Stability",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.IsIncrement = true;
                target.AddNumVariable = -amount;
            }
        );
        TakeDamage(m_tempStabilityData, isSound);
    }

    public void Heal(StabilityData stabilityData, bool isSound = true)
    {
        if (!m_ObjectT.IsAlive) return;
        if (!m_ObjectT.IsStabilityAlive) return;

        IsHeal = true;
        ChangeAllVariable(isSound, stabilityData);
    }

    public void TakeDamage(StabilityData stabilityData, bool isSound = true)
    {
        if (!m_ObjectT.IsAlive || !m_ObjectT.IsStabilityAlive) return;
        if (IsInvulnerable)
        {
            m_InvulnerableThingHappenTriggerSO.ObjecTThingHappen(m_ObjectT, m_InvulnerableRuntimeThingHappenData, string.Empty);
            return;
        }

        IsHit = true;
        ChangeAllVariable(isSound, stabilityData);
        CheckStability();
    }

    public override void PoisonHappened(ListPoisonData listPoisonData)
    {
        base.PoisonHappened(listPoisonData);
        CheckStability();
    }

    void CheckStability()
    {
        //if (StatsData.DataNumVars.FirstOrDefault(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase)).NumVariable <= 0)
        if (VariableFinder.GetVariableContainNameFromList(StatsData.DataNumVars, "Stability").NumVariable <= 0)
            Die();
    }

    public virtual void Die()
    {
        if (!m_ObjectT.IsAlive) return;
        if (!m_ObjectT.IsStabilityAlive) return;
        m_ObjectT.IsStabilityAlive = false;
        IsDie = true;

        //make health isCan = false , same as die
        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataBoolVariable>(
            this,
            StatsData.DataBoolVars,
            "Alive",
            dataBoolVar => dataBoolVar.IsCan = BoolNull.IsFalse
        );
        m_DieThingHappenTriggerSO.ObjecTThingHappen(
            m_ObjectT,
            m_DieRuntimeThingHappenData
        );
    }

    public void Revive(float healAmount = 0)
    {
        if (!m_ObjectT.IsStabilityAlive) IsRevive = true;
        else IsHeal = true;
        DataNumericalVariable stabilityDataVar = VariableFinder.GetVariableContainNameFromList(StatsData.DataNumVars, "Stability");
        //DataNumericalVariable healthDataVar = StatsData.DataNumVars.FirstOrDefault(v => v.Name.Equals(, StringComparison.OrdinalIgnoreCase));
        m_ObjectT.IsStabilityAlive = true;
        stabilityDataVar.AddNumVariable = healAmount > 0
        ? healAmount
        : stabilityDataVar.NumVariableMax;

        DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(m_ReviveStabilityData.DataNumVars, "Stability");
        source.AddNumVariable = stabilityDataVar.AddNumVariable;

        ChangeAllVariable(m_ReviveStabilityData);

        m_ReviveThingHappenTriggerSO.ObjecTThingHappen(
            m_ObjectT,
            m_ReviveRuntimeThingHappenData
        );
    }

    public void DrainStat(StatsMicrobarData statsMicrobarData)
    {
        if (statsMicrobarData is StabilityData stabilityData)
            TakeDamage(stabilityData, false);
    }
}
