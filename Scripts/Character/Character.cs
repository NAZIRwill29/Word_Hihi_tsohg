using System;
using UnityEngine;
using UnityEngine.Events;

public class Character : ObjectT, IActivable
{
    public ObjectShoot ObjectShoot;
    public ObjectMelee ObjectMelee;
    public ObjectEvadeable ObjectEvadeable;
    public ObjectMagic ObjectMagic;
    public ObjectAbility ObjectAbility;
    public ObjectDefense ObjectDefense;
    public ObjectSpeed ObjectSpeed;
    public ObjectStability ObjectStability;
    public ObjectVisibility ObjectVisibility;
    public NatureElement NatureElement { get; set; }
    public NatureElementControllerCharacter NatureElementControllerCharacter;
    public CharacterMovement CharacterMovement;
    public CharacterAudioMulti CharacterAudioMulti;
    //public bool IsCanMove => isCanMove;
    public HealthSystem HealthSystem;
    public StabilitySystem StabilitySystem;
    public ManaSystem ManaSystem;
    public Animator AdditionalAnim;
    public GameObject TopTransf, BotTransf, LeftTransf, RightTransf;
    [HideInInspector] public float OffsetRight, OffsetLeft, OffsetTop, OffsetBottom;
    [HideInInspector] public float OffsetRightTransf, OffsetLeftTransf, OffsetTopTransf, OffsetBottomTransf;
    [SerializeField] private bool m_IsActive;
    public bool IsActive
    {
        get => m_IsActive;
        set => m_IsActive = value;
    }

    [SerializeField] protected float m_StunnedDuration = 5;
    protected float m_StunnedTime;
    protected bool m_IsStunned;
    public bool IsStunned
    {
        get => m_IsStunned;
        set => m_IsStunned = value;
    }
    protected bool m_HasUnStunned;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    protected virtual void OnEnable()
    {
        if (!HealthSystem)
            return;
        HealthSystem.Died.AddListener(Died);
        HealthSystem.StatChanged.AddListener(SetAlive);
    }
    protected virtual void OnDisable()
    {
        if (!HealthSystem)
            return;
        HealthSystem.Died.RemoveListener(Died);
        HealthSystem.StatChanged.RemoveListener(SetAlive);
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsActive) return;

        m_StunnedTime -= Time.deltaTime;
        m_IsStunned = m_StunnedTime > 0;

        if (IsStunned) return;

        if (IsCanExecute && stateMachineScriptable != null)
        {
            stateMachineScriptable.Execute(this);
        }
        if (!IsAlive) return;
        UnStunned();

    }

    protected virtual void Initialize()
    {
        CharacterMovement = GetComponent<CharacterMovement>();

        OffsetRight = Math.Abs(RightTransf.transform.position.x - Center.transform.position.x);
        OffsetLeft = Math.Abs(LeftTransf.transform.position.x - Center.transform.position.x);
        OffsetTop = Math.Abs(TopTransf.transform.position.y - Center.transform.position.y);
        OffsetBottom = Math.Abs(BotTransf.transform.position.y - Center.transform.position.y);

        OffsetRightTransf = Math.Abs(RightTransf.transform.position.x - CenterTransf.transform.position.x);
        OffsetLeftTransf = Math.Abs(LeftTransf.transform.position.x - CenterTransf.transform.position.x);
        OffsetTopTransf = Math.Abs(TopTransf.transform.position.y - CenterTransf.transform.position.y);
        OffsetBottomTransf = Math.Abs(BotTransf.transform.position.y - CenterTransf.transform.position.y);
    }

    public void Activate(bool isTrue)
    {
        m_IsActive = isTrue;
        CharacterAudioMulti.MuteSound(!isTrue);
        Animator.enabled = isTrue;
    }

    // protected virtual void LateUpdate()
    // {
    // }

    protected virtual void Died()
    {
        IsAlive = false;
        //isCanMove = false;
    }
    protected virtual void SetAlive(float percentage, MicrobarAnimType microbarAnimType)
    {
        if (percentage > 0)
            IsAlive = true;
    }

    public override void RefreshNatureElement()
    {
        NatureElement = null;
        if (NatureElementControllerCharacter != null)
            NatureElementControllerCharacter.NatureElementEffectCharacterSOs.Clear();
    }

    public void Stunned()
    {
        m_StunnedTime = m_StunnedDuration;
        Debug.Log("Stunned");
        //OnStunned?.Invoke(m_StunnedDuration);
        AdditionalAnim.SetBool("Stunned", true);
        m_HasUnStunned = false;
    }

    protected void UnStunned()
    {
        if (m_HasUnStunned) return;
        m_HasUnStunned = true;
        AdditionalAnim.SetBool("Stunned", false);
    }
}
