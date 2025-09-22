using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ColliderType
{
    trigger, collider
}

//for object that can be damage
public class ObjectHealth : ObjectStat, IHealth, IDrain
{
    [SerializeField] protected ColliderType colliderType;
    [Optional][SerializeField] protected Rigidbody2D m_Rigidbody;
    [Optional][SerializeField] protected float m_DelayStopForce = 0.25f;
    public bool IsInvulnerable;
    public bool IsHit { get; set; }
    public bool IsHeal { get; set; }
    public bool IsRevive { get; set; }
    public bool IsDie { get; set; }
    [SerializeField] private HealthData m_tempHealthData;
    public UnityEvent<StatsMicrobarData> HealthChanged;
    [SerializeField] protected ThingHappenTriggerSO m_HealThingHappenTriggerSO, m_DamageThingHappenTriggerSO, m_InvulnerableThingHappenTriggerSO;
    [SerializeField] protected ThingHappenTriggerSO m_DieThingHappenTriggerSO, m_ReviveThingHappenTriggerSO;
    public float HealthDamageFactor = 1;

    protected HealthData m_ReviveHealthData = new()
    {
        DataNumVars = new() { new() {
                Name = "Health",
                IsIncrement = true,
                //NumVariable = healthDataVar.AddNumVariable
            } },
        DataBoolVars = new() { new() {
                Name = "Alive",
                IsCan = BoolNull.IsTrue
            } }
    };
    private ThingHappenData m_HealRuntimeThingHappenData = new();
    private ThingHappenData m_DamageRuntimeThingHappenData = new();
    private ThingHappenData m_InvulnerableRuntimeThingHappenData = new();
    private ThingHappenData m_DieRuntimeThingHappenData = new();
    private ThingHappenData m_ReviveRuntimeThingHappenData = new();


    //START IN INPECTOR
    //SETUP "alive" in StatsBoolChange

    public override void ChangeAllVariable(object data, bool inZone)
    {
        base.ChangeAllVariable(data, inZone);
        //Debug.Log("health() - ChangeAllVariable " + GetTime.GetCurrentTime("full-ms"));
        if (data is HealthData healthData)
            ChangeHealthVariable(healthData);
    }

    protected override void ChangeAllVariable(object data)
    {
        base.ChangeAllVariable(data);
        if (data is HealthData healthData)
            ChangeHealthVariable(healthData);
    }

    void ChangeHealthVariable(HealthData data)
    {
        if (data == null || StatsData == null) return;

        StatsData.EffectorName = data.OwnerName;

        VariableChanger.ChangeNameVariable(StatsData.DataNumVars, "Health",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                // Fetch source variable from data

                DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(data.DataNumVars, "Health");
                //DataNumericalVariable source = data.DataNumVars.FirstOrDefault(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase));

                // If source is null, exit early
                if (source == null)
                    return;

                // Get index of target variable, can be used for logging if necessary
                int id = data.DataNumVars.FindIndex(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase));

                if (target.Cooldown <= 0)
                {
                    // Update target variable based on source
                    target.IsCanRecovery = source.IsCanRecovery != BoolNull.IsNull ? source.IsCanRecovery : target.IsCanRecovery;
                    //Debug.Log("AddNumVariable 3 " + source.AddNumVariable);
                    if (source.AddNumVariable < 0)
                        target.AddNumVariable = source.AddNumVariable * HealthDamageFactor;
                    else
                        target.AddNumVariable = source.AddNumVariable;

                    target.NumVariable = source.IsIncrement
                        ? target.NumVariable + target.AddNumVariable
                        : target.NumVariableOri + target.AddNumVariable;
                    Debug.Log("HChangeNumVariable " + target.Name + " " + target.NumVariable + " : add " + target.AddNumVariable);

                    target.AddNumVariableMax = source.AddNumVariableMax;
                    target.NumVariableMax = source.IsIncrement
                        ? target.NumVariableMax + target.AddNumVariableMax
                        : target.NumVariableMaxOri + target.AddNumVariableMax;

                    target.NumVariableOri = source.IsIncrement ? target.NumVariable : target.NumVariableOri;
                    target.NumVariableMaxOri = source.IsIncrement ? target.NumVariableMax : target.NumVariableMaxOri;

                    target.TimeCoolDown = source.TimeCoolDown > 0 ? source.TimeCoolDown : target.TimeCoolDown;
                    target.Cooldown = target.TimeCoolDown;

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
                    HealthData healthData = new()
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

                    Debug.Log("HealthChanged Invoke");
                    HealthChanged.Invoke(healthData);
                }
            }
        );
    }

    public void TakeDamage(int amount)
    {
        // Validate the amount
        if (amount <= 0)
            return;

        // Initialize HealthData with a single numerical variable
        VariableChanger.ChangeNameVariable(m_tempHealthData.DataNumVars, "Health",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.IsIncrement = true;
                target.AddNumVariable = -amount;
            }
        );
        TakeDamage(m_tempHealthData);
    }
    public void Heal(int amount)
    {
        // Validate the amount
        if (amount <= 0)
            return;

        // Initialize HealthData with a single numerical variable
        HealthData healthData = new()
        {
            DataNumVars = new List<DataNumericalVariable> { new DataNumericalVariable() {
                Name = "Health",
                IsIncrement = true,
                AddNumVariable = amount
            } }
        };
        Heal(healthData);
    }

    public virtual void TakeDamage(HealthData healthData)
    {
        if (!m_ObjectT.IsAlive) return;
        if (IsInvulnerable)
        {
            m_InvulnerableThingHappenTriggerSO.ObjecTThingHappen(m_ObjectT, m_InvulnerableRuntimeThingHappenData, string.Empty);
            return;
        }
        //Debug.Log("TakeDamage");
        IsHit = true;
        ChangeAllVariable(healthData);
        CheckHealth();
        Invoke("StopPushForce", m_DelayStopForce);
        // m_ObjectT.ThingHappen(new()
        // {
        //     SoundName = "Damage",
        //     FXName = m_ObjectT.ObjectTType + "DamageFX",
        // });
    }

    public void Heal(HealthData healthData)
    {
        if (!m_ObjectT.IsAlive) return;

        IsHeal = true;
        ChangeAllVariable(healthData);
        // m_ObjectT.ThingHappen(new()
        // {
        //     SoundName = "Heal",
        //     FXName = "HealFX"
        // });
    }

    public override void PoisonHappened(ListPoisonData listPoisonData)
    {
        base.PoisonHappened(listPoisonData);
        CheckHealth();
    }

    void CheckHealth()
    {
        //if (StatsData.DataNumVars.FirstOrDefault(v => v.Name.Equals("Health", StringComparison.OrdinalIgnoreCase)).NumVariable <= 0)
        if (VariableFinder.GetVariableContainNameFromList(StatsData.DataNumVars, "Health").NumVariable <= 0)
            Die();
    }

    public virtual void Die()
    {
        if (!m_ObjectT.IsAlive) return;
        //m_ObjectT.IsAlive = false;
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
        if (!m_ObjectT.IsAlive) IsRevive = true;
        else IsHeal = true;
        DataNumericalVariable healthDataVar = VariableFinder.GetVariableContainNameFromList(StatsData.DataNumVars, "Health");
        //DataNumericalVariable healthDataVar = StatsData.DataNumVars.FirstOrDefault(v => v.Name.Equals(, StringComparison.OrdinalIgnoreCase));
        m_ObjectT.IsAlive = true;
        healthDataVar.AddNumVariable = healAmount > 0
        ? healAmount
        : healthDataVar.NumVariableMax;

        DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(m_ReviveHealthData.DataNumVars, "Health");
        source.AddNumVariable = healthDataVar.AddNumVariable;

        ChangeAllVariable(m_ReviveHealthData);
        m_ReviveThingHappenTriggerSO.ObjecTThingHappen(
            m_ObjectT,
            m_ReviveRuntimeThingHappenData
        );
    }

    void StopPushForce()
    {
        if (colliderType == ColliderType.trigger)
            return;
        m_Rigidbody.linearVelocity = Vector3.zero;
        m_Rigidbody.angularVelocity = 0;
    }

    public void DrainStat(StatsMicrobarData statsMicrobarData)
    {
        if (statsMicrobarData is HealthData healthData)
            TakeDamage(healthData);
    }
}
