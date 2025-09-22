using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WordBar
{
    [SerializeField] private NameDataFlyweight m_NameDataFlyweight;
    public string Name { get => m_NameDataFlyweight.Name; }
}

public class CombatSystem : MonoBehaviour
{
    public ObjectT GhostT;
    [SerializeField] private PlayModeManager m_PlayModeManager;
    public WordCheck WordCheck;
    public EscapeWordSystem EscapeWordSystem;
    //public CombatDistanceSystem CombatDistanceSystem;
    public CombatDataManager CombatDataManager;
    public ExorcismLetterSystem ExorcismLetterSystem;
    public LetterPlacementSystem LetterPlacementSystem;
    public LetterRuleSystem LetterRuleSystem;
    public CombatTypingSystem CombatTypingSystem;
    public ExorcismWordSystem ExorcismWordSystem;
    public WeaknessWordSystem WeaknessWordSystem;
    public CombatStarter CombatStarter;
    public CombatUI CombatUI;
    public GhostCombatDataFlyweight GhostCombatData { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public StabilityStatDrainOverTime GhostStabilityStatDrainOverTime;
    public StabilityStatDrainOverTime PlayerStabilityStatDrainOverTime;
    public StabilityStateManager GhostStabilityStateManager;
    public StabilityStateManager PlayerStabilityStateManager;
    public HealthStateManager GhostHealthStateManager;
    public HealthStateManager PlayerHealthStateManager;
    public float ChangeRuleCooldown { get; private set; }
    public float ChangeRuleTime { get; private set; }
    private bool m_CanStartChangeRuleCountdown;
    [SerializeField] private ListWrapper<WordBar> m_WordBars;
    private int m_WordBarIndex;
    public bool IsTypingMode { get; private set; }
    public bool IsStart { get; private set; }
    public bool IsEnd { get; private set; }
    [SerializeField] private float SuccesLetterRuleManaNum = 3, FailedLetterRuleManaNum = 2;
    [SerializeField] private NameDataFlyweight ActivePlayModeNameData;
    private List<GameOverData> m_GameOverDatas = new();
    public UnityEvent<int> OnChangeWordBar;
    public UnityEvent<bool> OnChangeMode;
    public UnityEvent OnEscapeCombat, OnStartCombat, OnSuccessFacingStruggleMode, OnFailedFacingStruggleMode;
    public UnityEvent<ActiveAbilityData> OnActiveAbility;
    public UnityEvent<PassiveAbilityData> OnPassiveAbility;
    public UnityEvent OnHideGhostPrompt, OnStopGhostPrompt, OnExitCombat;
    public UnityEvent<GhostTemplate> OnStartGhostPrompt, OnShowGhostPrompt;

    void OnEnable()
    {
        if (m_PlayModeManager)
            m_PlayModeManager.OnPlayModeChange.AddListener(OnPlayModeChange);

        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.AddListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.AddListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.AddListener(OnGhostZeroStability);
        }

        if (PlayerStabilityStateManager)
        {
            PlayerStabilityStateManager.OnLowStability.AddListener(CombatTypingSystem.OnLowStability);
            PlayerStabilityStateManager.OnHighStability.AddListener(CombatTypingSystem.OnHighStability);
        }

        // if (PlayerStabilityStateManager)
        // {
        //     PlayerStabilityStateManager.OnZeroStability.AddListener(OnPlayerZeroStability);
        // }

        if (LetterRuleSystem)
        {
            LetterRuleSystem.OnSuccesLetterRule.AddListener(OnSuccesLetterRule);
            LetterRuleSystem.OnFailedLetterRule.AddListener(OnFailedLetterRule);
        }

        if (CombatTypingSystem)
        {
            CombatTypingSystem.OnCantBackspace.AddListener(OnCantBackspace);
        }
    }

    void OnDisable()
    {
        if (m_PlayModeManager)
            m_PlayModeManager.OnPlayModeChange.RemoveListener(OnPlayModeChange);

        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.RemoveListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.RemoveListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.RemoveListener(OnGhostZeroStability);
        }

        if (PlayerStabilityStateManager)
        {
            PlayerStabilityStateManager.OnLowStability.RemoveListener(CombatTypingSystem.OnLowStability);
            PlayerStabilityStateManager.OnHighStability.RemoveListener(CombatTypingSystem.OnHighStability);
        }

        // if (PlayerStabilityStateManager)
        // {
        //     PlayerStabilityStateManager.OnZeroStability.RemoveListener(OnPlayerZeroStability);
        // }

        if (LetterRuleSystem)
        {
            LetterRuleSystem.OnSuccesLetterRule.RemoveListener(OnSuccesLetterRule);
            LetterRuleSystem.OnFailedLetterRule.RemoveListener(OnFailedLetterRule);
        }

        if (CombatTypingSystem)
        {
            CombatTypingSystem.OnCantBackspace.RemoveListener(OnCantBackspace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsStart) return;
        if (!m_CanStartChangeRuleCountdown) return;
        if (GhostCombatSystem.Instance.ActiveStruggleMode) return;

        ChangeRuleCooldown -= Time.deltaTime;
        if (ChangeRuleCooldown <= 0)
        {
            ChangeRuleCooldown = ChangeRuleTime;
            ChangeRuleRandomly();
        }
    }

    private void OnPlayModeChange(string name)
    {
        Activate(ActivePlayModeNameData.Name == name);
    }

    public void Activate(bool isTrue)
    {
        Debug.Log("Activate " + isTrue);
        GameManager.Instance.IsNeedGameOver = false;
        GameManager.Instance.player2D.ObjectSpeed.CanWalkChange(false);

        if (CombatDataManager)
        {
            if (CombatDataManager.Player2D)
            {
                CombatDataManager.Player2D.InBattle = isTrue;
                CombatDataManager.Player2D.ObjectHealth.HealthDamageFactor = 1;
            }
            if (CombatDataManager.Ghost)
            {
                CombatDataManager.Ghost.InBattle = isTrue;
                //make standby
                CombatDataManager.Ghost.ObjectSpeed.IsStandBy = isTrue;
            }

            m_GameOverDatas.Clear();
            foreach (var item in CombatDataManager.GhostTemplate.GameOverDatas)
            {
                m_GameOverDatas.Add(item);
            }
            GhostCombatData = CombatDataManager.GhostCombatDataFlyweight;
            WordCheck.ClearSuccessWord();
            IsTypingMode = true;
            ChangeWordBar("ExorcismLetter");

            if (isTrue)
            {
                OnStartGhostPrompt?.Invoke(CombatDataManager.GhostTemplate);
                CombatDataManager.Ghost.HealthSystem.Died.AddListener(GhostDeath);
                CombatDataManager.Player2D.HealthSystem.Died.AddListener(PlayerDeath);
                CombatDataManager.Player2D.StabilitySystem.Died.AddListener(PlayerMindBreak);
            }
            else
            {
                CombatDataManager.Ghost.HealthSystem.Died.RemoveListener(GhostDeath);
                CombatDataManager.Player2D.HealthSystem.Died.RemoveListener(PlayerDeath);
                CombatDataManager.Player2D.StabilitySystem.Died.RemoveListener(PlayerMindBreak);
            }
        }
    }

    //start combat - once on beginning
    //call with space key - after activate
    public void StartCombat()
    {
        if (IsStart) return; //prevent when combat start
        if (IsEnd) return; //prevent when combat end
        //only allowed when combat not started yet
        IsStart = true;
        IsEnd = false;
        OnHideGhostPrompt?.Invoke();

        ChangeRuleTime = GhostCombatData.ChangeRuleTime;
        ChangeRuleCooldown = ChangeRuleTime;

        //Debug.Log("StartCombat");
        m_CanStartChangeRuleCountdown = true;

        CombatTypingSystem.StartSystem(true);
        EscapeWordSystem.StartCombat();
        ExorcismWordSystem.StartCombat();
        WeaknessWordSystem.StartCombat();
        //CombatDistanceSystem.StartCombat(CombatDataManager.GhostTemplate);
        GhostCombatSystem.Instance.StartCombat(CombatDataManager.GhostTemplate, CombatDataManager.ObjectT, CombatDataManager.GhostCombatDataFlyweight);
        ChangeRuleRandomly();
        OnStartCombat?.Invoke();

        CombatDataManager.Ghost.Activate(true);
        //make mana zero
        if (CombatDataManager.Ghost.ObjectMagic)
        {
            VariableChanger.ChangeNameVariable(CombatDataManager.Ghost.ObjectMagic.StatsData.DataNumVars, "Mana",
                (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
                (target) =>
                {
                    target.NumVariable = 0;
                }
            );
        }

        if (PlayerStabilityStatDrainOverTime)
        {
            // PlayerStabilityStateManager.Activate(CombatDataManager.Player2D);
            // PlayerHealthStateManager.Activate(CombatDataManager.Player2D);
            PlayerStabilityStatDrainOverTime.Activate(true);
        }
        if (GhostStabilityStatDrainOverTime)
        {
            GhostStabilityStateManager.Activate(CombatDataManager.Ghost);
            GhostHealthStateManager.Activate(CombatDataManager.Ghost);
            GhostStabilityStatDrainOverTime.ObjectStability = CombatDataManager.Ghost.ObjectStability;
            GhostStabilityStatDrainOverTime.Activate(true);
        }

        if (CombatDataManager.Player2D.ObjectHealth is PlayerHealth playerHealth)
        {
            PlayerHealth = playerHealth;
            PlayerHealth.ResetCameraShakeIntensity();
        }
    }

    public void ChangeRuleRandomly()
    {
        LetterRuleSystem.SetRandomLetterRule();
        LetterPlacementSystem.SetRandomLetterPlacement();
    }

    public void EscapeWordBar()
    {
        if (!IsStart) return;
        ChangeWordBar("EscapeWord");
    }

    public void WeaknessWordBar()
    {
        if (!IsStart) return;
        ChangeWordBar("WeaknessWord");
    }

    public void ExorcismLetterBar()
    {
        if (!IsStart) return;
        ChangeWordBar("ExorcismLetter");
    }

    public void InventoryBar()
    {
        if (!IsStart) return;
        ChangeWordBar("Inventory");
    }

    public void ShiftChangeWordBar()
    {
        if (!IsStart) return;
        m_WordBarIndex++;
        if (m_WordBarIndex > 3)
            m_WordBarIndex = 0;
        OnChangeWordBar?.Invoke(m_WordBarIndex);
    }

    public void ChangeWordBar(string barName)
    {
        if (m_WordBars.TryGetIndex(x => x.Name == barName, out int index))
        {
            m_WordBarIndex = index;
            OnChangeWordBar?.Invoke(m_WordBarIndex);
        }
    }

    public void InitConfirmWord()
    {
        if (!IsStart) return;
        CombatTypingSystem.InitConfirmWord();
    }

    public void ChangeMode()
    {
        if (!IsStart) return;
        IsTypingMode = !IsTypingMode;
        Debug.Log(IsTypingMode);
        OnChangeMode?.Invoke(IsTypingMode);
    }

    public void EscapeCombat()
    {
        if (GhostCombatSystem.Instance.ActiveStruggleMode)
        {
            CombatUI.ShowAbilityPrompt2(true, "Can't escape when struggling with ghost");
            return;
        }
        if (!IsStart) return;
        IsEnd = true;
        CombatUI.OnShowGhostPrompt(
            CombatDataManager.GhostTemplate.Appearance,
            CombatDataManager.GhostTemplate.OnFlee,
            CombatDataManager.GhostTemplate.PromptBoxSprite,
            CombatDataManager.GhostTemplate.PromptUISprite
        );
        CombatDataManager.GhostTemplate.EscapeNum--;
        CombatUI.ChangeEscapeIcon(CombatDataManager.GhostTemplate.EscapeNum);
        OnEscapeCombat?.Invoke();
        // make ghost stunned
        CombatDataManager.Ghost.Stunned();
        //CombatDataManager.Ghost.gameObject.SetActive(false);
        ExitCombat();
    }

    public void ExitCombat()
    {
        if (!IsStart) return; //prevent when combat not started yet
        if (!IsEnd) return; //prevent when combat not ended yet
        //only allowed when combat is end
        PlayerStabilityStatDrainOverTime.Activate(false);
        GhostStabilityStatDrainOverTime.Activate(false);
        // PlayerStabilityStateManager.Deactivate();
        // GhostStabilityStateManager.Deactivate();

        CombatStarter.StartCooldown();
        m_CanStartChangeRuleCountdown = false;
        GhostCombatSystem.Instance.ExitCombat();
        CombatTypingSystem.StartSystem(false);
        IsStart = false;
        OnStopGhostPrompt?.Invoke();
        OnExitCombat?.Invoke();
        GameManager.Instance.player2D.ObjectSpeed.CanWalkChange(true);
        GameManager.Instance.ChangePlayMode(m_PlayModeManager.NormalListenerNameData.Name);
        KeyboardManager.Instance.ResetBannedLetterKeys();

        GameManager.Instance.MiniGameManager.StartCooldown();
        GameManager.Instance.MiniGameManager.CanMiniGame = true;
        CombatDataManager.Ghost.ObjectSpeed.IsStandBy = false;
        CombatDataManager.Ghost.IsCanExecute = true;
        GameManager.Instance.HidingMechanic.HidingPlace.ForceInteractInPhaseOne();
    }

    //shaman path
    private void GhostDeath()
    {
        IsEnd = true;
        CombatUI.OnShowGhostPrompt(
            CombatDataManager.GhostTemplate.Appearance,
            CombatDataManager.GhostTemplate.OnDeath,
            CombatDataManager.GhostTemplate.PromptBoxSprite,
            CombatDataManager.GhostTemplate.PromptUISprite
        );
        CombatDataManager.Ghost.gameObject.SetActive(false);
    }

    //ustaz path
    public void GhostBanished()
    {
        IsEnd = true;
        CombatUI.OnShowGhostPrompt(
            CombatDataManager.GhostTemplate.Appearance,
            CombatDataManager.GhostTemplate.OnBanished,
            CombatDataManager.GhostTemplate.PromptBoxSprite,
            CombatDataManager.GhostTemplate.PromptUISprite
        );
        //TODO() EXTRA() - make ghost fly or disappear while reconcile with player
        CombatDataManager.Ghost.gameObject.SetActive(false);
    }

    private void PlayerDeath()
    {
        IsEnd = true;
        CombatUI.OnShowGhostPrompt(
            CombatDataManager.GhostTemplate.Appearance,
            CombatDataManager.GhostTemplate.OnFailHealth,
            CombatDataManager.GhostTemplate.PromptBoxSprite,
            CombatDataManager.GhostTemplate.PromptUISprite
        );
        GameManager.Instance.GameOverData = m_GameOverDatas.Find(x => x.NameData.Name == "PlayerDeath");
        GameManager.Instance.IsNeedGameOver = true;
    }

    private void PlayerMindBreak()
    {
        IsEnd = true;
        CombatUI.OnShowGhostPrompt(
            CombatDataManager.GhostTemplate.Appearance,
            CombatDataManager.GhostTemplate.OnFailStability,
            CombatDataManager.GhostTemplate.PromptBoxSprite,
            CombatDataManager.GhostTemplate.PromptUISprite
        );
        GameManager.Instance.GameOverData = m_GameOverDatas.Find(x => x.NameData.Name == "PlayerMindBreak");
        GameManager.Instance.IsNeedGameOver = true;
    }

    private void OnGhostLowStability(string name)
    {
        CombatDataManager.Ghost.ObjectMagic.ChangeRecoveryMana(
            CombatDataManager.GhostTemplate.RecoveryManaAnger,
            10,
            true,
            true
        );
    }

    private void OnGhostHighStability(string name)
    {
        CombatDataManager.Player2D.ObjectHealth.HealthDamageFactor = 1;
        CombatDataManager.Ghost.ObjectMagic.IsInvulnerable = false;
        CombatDataManager.Ghost.ObjectMagic.ChangeRecoveryMana(
            CombatDataManager.GhostTemplate.RecoveryManaCalm,
            10,
            false,
            true
        );
    }

    private void OnGhostZeroStability(string name)
    {
        OnGhostLowStability(name);
        CombatDataManager.Player2D.ObjectHealth.HealthDamageFactor = 2;
        CombatDataManager.Ghost.ObjectMagic.IsInvulnerable = true;
    }

    private void OnCantBackspace()
    {
        CombatUI.ShowAbilityPrompt2(true, "Can't use backspace in fear");
    }

    // private void OnPlayerZeroStability(string name)
    // {
    //     //TODO() - GAME OVER - POSSESSED
    // }

    private void OnSuccesLetterRule()
    {
        CombatDataManager.Ghost.ObjectMagic.TakeDamage(SuccesLetterRuleManaNum);
        CombatUI.ShakeGhost();
        GhostCombatSystem.Instance.CombatUI.GhostCharAnimation("Hitted");
    }

    private void OnFailedLetterRule()
    {
        CombatDataManager.Ghost.ObjectMagic.Heal(FailedLetterRuleManaNum);
    }
}
