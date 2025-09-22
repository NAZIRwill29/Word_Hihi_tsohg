using System;
using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : StatsTrigger
{
    protected float m_Cooldown;
    //[SerializeField] protected float m_TimeCoolDown = 1;
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected TriggerStay m_TriggerStay;
    [SerializeField] protected TriggerExit m_TriggerExit;
    public event UnityAction<Collider2D> OnTriggerEffect, OnTriggerExit;

    protected void OnEnable()
    {
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;
    }
    protected void OnDisable()
    {
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D -= TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D -= TriggerExit;
    }

    protected override void ProcessTrigger(Collider2D other)
    {
        //(1) trigger when Checker enter -> call triggered in stats of footChecker
        //ex: DefenseZone - call triggered(DefenseEffect, DefenseEffectData) in CharacterStatsManager of footChecker
        m_Cooldown -= Time.deltaTime;
        if (m_Cooldown > 0)
            return;

        OnTriggerEffect?.Invoke(other);

        Receiver receiver = other.GetComponent<Receiver>();
        if (receiver.Name != m_StatTriggerFlyweightData.Receiver)
        {
            Debug.LogWarning($"Checker of type {m_StatTriggerFlyweightData.Receiver} is null or missing ObjectStatsManager!");
            return;
        }
        //Debug.Log("health() - ProcessTrigger " + GetTime.GetCurrentTime("full-ms"));
        if (m_StatTriggerFlyweightData is ZoneTriggerFlyweight zoneTriggerFlyweight)
            m_Cooldown = zoneTriggerFlyweight.TimeCoolDown;
        var effectData = CreateEffectData();
        if (effectData != null)
        {
            receiver.ObjectStatsManager.Triggered(other, effectData, true);
        }
        else
        {
            Debug.LogWarning("Effect data is null!");
        }
    }

    protected override void ProcessExit(Collider2D other)
    {
        OnTriggerExit?.Invoke(other);
        //m_Cooldown -= Time.deltaTime;
        base.ProcessExit(other);
    }
}
