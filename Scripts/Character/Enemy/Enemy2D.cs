using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy2DMovement), typeof(CharacterAudioMulti))]
public class Enemy2D : Character
{
    [SerializeField] protected Enemy2DAI m_Enemy2DAI;
    protected Enemy2DMovement m_Enemy2DMovement;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public DetectionSense DetectionSense;
    [HideInInspector] public bool IsChase, IsPlayerEscape, IsDetectPlayer, IsAfterTrack;
    public EnemyAISO ActiveEnemyAISO;
    public AttackSO AttackSO;
    public float AddLineDetection { get; set; }
    public float TrackCooldown { get; set; }
    public float AttackCooldown { get; set; }
    public float AttackChangeDurationCooldown { get; set; }
    public float AttackAnimCooldown { get; set; }
    [HideInInspector] public float GoalDist, StaticCd, ChangeGoalCd;
    [HideInInspector] public Vector3 PlayerPrevPos;
    public GameObject TempTarget;
    public bool IsStartIdle;
    public bool IsIdle;
    public bool IsFlee { get; set; }
    public bool IsTrack { get; set; }
    public float AttackLine { get; set; }
    [SerializeField] private Gun m_Gun;
    [SerializeField] private EnemyRuntimeSetSO RuntimeSet;
    protected bool m_HasPlayerEscape;
    //protected bool m_HasDetectPlayer;
    // [SerializeField] protected float m_HasDetectPlayerCooldownDuration = 1;
    // protected float m_HasDetectPlayerCooldownTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (ObjectHealth) ObjectHealth.HealthChanged.AddListener(NotifyHealthChanged);
        if (RuntimeSet) RuntimeSet.Add(this);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (HealthSystem) ObjectHealth.HealthChanged.RemoveListener(NotifyHealthChanged);
        if (RuntimeSet) RuntimeSet.Remove(this);
    }

    protected override void Initialize()
    {
        base.Initialize();
        m_Enemy2DMovement = GetComponent<Enemy2DMovement>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        IsIdle = IsStartIdle;
        m_Enemy2DAI.Initialize();
        m_Gun.Target = GameManager.Instance.player2D.Center;
    }

    protected override void Start()
    {
        base.Start();
        ActiveEnemyAISO.SetTarget(m_Enemy2DMovement, this, NavMeshAgent);
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsActive) return;
        float deltaTime = Time.deltaTime;

        m_StunnedTime -= deltaTime;
        m_IsStunned = m_StunnedTime > 0;

        if (IsStunned) return;

        if (IsCanExecute && stateMachineScriptable != null)
        {
            stateMachineScriptable.Execute(this);
        }
        if (!IsAlive) return;
        UnStunned();
        if (!m_Enemy2DMovement.IsCanWalk) return;

        IsDetectPlayer = DetectionSense.DetectPlayer(AddLineDetection);
        DetectPlayerAnimation(IsDetectPlayer);
        m_Enemy2DAI.Execute(this, m_Enemy2DMovement);

        // m_HasDetectPlayerCooldownTime -= deltaTime;
        // if (m_HasDetectPlayerCooldownTime <= 0 && !IsDetectPlayer)
        //     m_HasDetectPlayer = false;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsActive) return;
        if (!IsAlive) return;
        if (IsStunned) return;
        m_Enemy2DAI.FixedCheck(this, m_Enemy2DMovement);
    }

    void NotifyHealthChanged(StatsMicrobarData statsMicrobarData)
    {
        if (statsMicrobarData is HealthData healthData)
            m_Enemy2DAI.NotifyHealthChanged(this, healthData);
    }

    public override void ThingHappen(ThingHappenData thingHappenData)
    {
        if (!IsActive) return;
        if (IsStunned) return;
        //Debug.Log("ThingHappen");
        base.ThingHappen(thingHappenData);
    }

    public void ForceEnemyAIStateChange(string name)
    {
        m_Enemy2DAI.SetEnemyState(this, m_Enemy2DMovement, name);
    }

    protected void DetectPlayerAnimation(bool isTrue)
    {
        if (!isTrue) return;
        //if (m_HasDetectPlayer) return;
        if (!IsPlayerEscape) return;
        AdditionalAnim.SetTrigger("Notice");
        // m_HasDetectPlayerCooldownTime = m_HasDetectPlayerCooldownDuration;
        m_HasPlayerEscape = false;
        IsPlayerEscape = false;
    }

    public void PlayerEscapeAnimation()
    {
        if (m_HasPlayerEscape) return;
        AdditionalAnim.SetTrigger("Question");
        m_HasPlayerEscape = true;
    }
}
