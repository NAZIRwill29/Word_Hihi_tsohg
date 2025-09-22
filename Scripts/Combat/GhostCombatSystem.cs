using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AbilityPassData
{
    [ExpandableTextArea] public string RuleText;
    public List<string> Texts = new();
    public string EffectImageAnimationConstant;
    public Sprite GhostProfSprite;
    public Sprite AbilityPromptUISprite;
    public Sprite AbilityPromptTextContSprite;
}
[System.Serializable]
public class ActiveAbilityData : AbilityPassData
{
    public Sprite BackgroundSprite;
}
[System.Serializable]
public class PassiveAbilityData : AbilityPassData
{
    public Sprite GhostCharSprite;
    public List<Sprite> ExorcismLetterCoverSprites = new();
}

public class GhostCombatSystem : Singleton<GhostCombatSystem>
{
    public CombatSystem CombatSystem;
    [SerializeField] private GhostTemplate m_GhostTemplate;
    public GhostTemplate GhostTemplate => m_GhostTemplate;
    public CombatUI CombatUI;// { get; set; }
    public WordCheck WordCheck;
    public WordSystem WordSystem;
    private ObjectT m_ObjectT;
    [SerializeField] private GhostAbility m_GhostAbility;
    public GhostAbility GhostAbility { get => m_GhostAbility; }
    private bool m_IsAdvanceCombat;
    public float PassiveChangeAbilityCooldown { get; set; }
    public float PassiveChangeAbilityTime { get; set; }
    public float ChangeAbilityRestCooldown { get; set; }
    public float ChangeAbilityRestTime { get; set; }
    public float ActiveStruggleModeCooldown { get; set; }
    public float ActiveStruggleModeTime { get; set; }
    private bool m_CanStartCombatCountdown;
    private bool m_CanStartCountdown;
    public bool ActiveStruggleMode { get; set; }

    // private void OnEnable()
    // {
    //     if (!CombatSystem) return;

    //     if (CombatSystem.CombatDataManager.Ghost.ObjectMagic)
    //     {
    //         CombatSystem.CombatDataManager.Ghost.ObjectMagic.ManaChanged.AddListener(ManaChanged);
    //     }

    //     // if (CombatSystem.CombatDistanceSystem)
    //     // {
    //     //     CombatSystem.CombatDistanceSystem.OnReachPlayer.AddListener(StartActiveStruggleMode);
    //     // }
    // }

    private void OnDisable()
    {
        if (!CombatSystem) return;

        if (CombatSystem.CombatDataManager.Ghost && CombatSystem.CombatDataManager.Ghost.ObjectMagic)
        {
            CombatSystem.CombatDataManager.Ghost.ObjectMagic.ManaChanged.RemoveListener(ManaChanged);
        }


        // if (CombatSystem.CombatDistanceSystem)
        // {
        //     CombatSystem.CombatDistanceSystem.OnReachPlayer.RemoveListener(StartActiveStruggleMode);
        // }
    }

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!CombatSystem.IsStart) return;
        if (CombatSystem.IsEnd) return;
        if (!m_CanStartCountdown) return;

        // if (ChangeAbilityRestCooldown > -1)
        // {
        if (!m_CanStartCombatCountdown)
            ChangeAbilityRestCooldown -= Time.deltaTime;
        if (ChangeAbilityRestCooldown < 0)
        {
            m_CanStartCombatCountdown = true;
            //StartPassiveStruggleMode();
        }
        //}

        if (!m_CanStartCombatCountdown) return;

        if (m_GhostAbility)
            m_GhostAbility.DoUpdate();

        if (!ActiveStruggleMode)
        {
            PassiveChangeAbilityCooldown -= Time.deltaTime;
            if (PassiveChangeAbilityCooldown < 0)
            {
                PassiveChangeAbilityCooldown = PassiveChangeAbilityTime;
                StartPassiveStruggleMode();
            }
        }
        else
        {
            ActiveStruggleModeCooldown -= Time.deltaTime;
            if (ActiveStruggleModeCooldown < 0)
            {
                ActiveStruggleModeCooldown = ActiveStruggleModeTime;
                //ExitActiveStruggleMode();
                m_GhostAbility.DoPunishment();
            }
        }
    }

    public void StartCombat(GhostTemplate ghostTemplate, ObjectT objectT, GhostCombatDataFlyweight ghostCombatDataFlyweight)
    {
        m_GhostTemplate = ghostTemplate;
        m_ObjectT = objectT;
        //ChangeAbilityTime = ghostCombatDataFlyweight.ChangeAbilityTime;
        ChangeAbilityRestTime = ghostCombatDataFlyweight.ChangeAbilityRestTime;
        //ActiveStruggleModeTime = ghostCombatDataFlyweight.ActiveStruggleModeTime;
        //ChangeAbilityCooldown = ChangeAbilityTime;
        ChangeAbilityRestCooldown = ChangeAbilityRestTime;
        ActiveStruggleModeCooldown = ActiveStruggleModeTime;
        m_CanStartCountdown = true;
        //StartPassiveStruggleMode();

        if (!CombatSystem) return;

        if (CombatSystem.CombatDataManager.Ghost && CombatSystem.CombatDataManager.Ghost.ObjectMagic)
        {
            CombatSystem.CombatDataManager.Ghost.ObjectMagic.ManaChanged.AddListener(ManaChanged);
            ObjectStatProcessor.GetUnityEventInStatNumChange(
                CombatSystem.CombatDataManager.Ghost.ObjectMagic,
                "Mana"
            ).AddListener(ManaChanged);
        }
    }

    public void ChangeAdvanceMode(bool isTrue)
    {
        m_IsAdvanceCombat = isTrue;
    }

    private void ManaChanged(StatsMicrobarData statsMicrobarData)
    {
        if (VariableFinder.GetVariableContainNameFromList(statsMicrobarData.DataNumVars, "Mana").NumVariable >= 100)
        {
            StartActiveStruggleMode();
        }
    }

    private void ManaChanged(DataNumericalVariable dataNumericalVariable)
    {
        if (dataNumericalVariable.NumVariable >= 100)
        {
            StartActiveStruggleMode();
        }
    }

    public void StartActiveStruggleMode()
    {
        if (!m_GhostTemplate) return;

        m_GhostAbility.ExitAbility();
        ExitPassiveStruggleMode();

        if (!m_IsAdvanceCombat)
            m_GhostAbility = m_GhostTemplate.ActiveGhostAbilities[Random.Range(0, m_GhostTemplate.ActiveGhostAbilities.Count)];
        else
            m_GhostAbility = m_GhostTemplate.AdvanceActiveGhostAbilities[Random.Range(0, m_GhostTemplate.ActiveGhostAbilities.Count)];

        ActiveStruggleModeTime = m_GhostAbility.AbilityTime;
        ActiveStruggleModeCooldown = ActiveStruggleModeTime;
        CombatSystem.CombatDataManager.Ghost.ObjectMagic.IsStopRecovery = true;
        m_GhostAbility.Use(CombatSystem.CombatDataManager.Ghost);
        CombatUI.ShakeUI();
    }

    // private void StartPassiveStruggleMode()
    // {
    //     if (!m_GhostTemplate) return;
    //     if (m_GhostAbility)
    //         m_GhostAbility.ExitAbility();

    //     if (!m_IsAdvanceCombat)
    //         m_GhostAbility = m_GhostTemplate.PassiveGhostAbilities[Random.Range(0, m_GhostTemplate.PassiveGhostAbilities.Count)];
    //     else
    //         m_GhostAbility = m_GhostTemplate.AdvancePassiveGhostAbilities[Random.Range(0, m_GhostTemplate.PassiveGhostAbilities.Count)];

    //     PassiveChangeAbilityTime = m_GhostAbility.AbilityTime;
    //     PassiveChangeAbilityCooldown = PassiveChangeAbilityTime;
    //     CombatSystem.CombatDataManager.Ghost.ObjectMagic.IsStopRecovery = false;
    //     m_GhostAbility.Use(m_ObjectT);
    //     CombatUI.ShakeUI();
    // }

    private void StartPassiveStruggleMode()
    {
        if (!m_GhostTemplate) return;

        // Exit previous ability if any
        if (m_GhostAbility != null)
            m_GhostAbility.ExitAbility();

        // Select a new passive ghost ability based on combat mode
        List<PassiveGhostAbility> abilityList = m_IsAdvanceCombat
            ? m_GhostTemplate.AdvancePassiveGhostAbilities
            : m_GhostTemplate.PassiveGhostAbilities;

        // Prevent errors if the list is empty
        if (abilityList == null || abilityList.Count == 0)
        {
            Debug.LogWarning("No passive ghost abilities available.");
            return;
        }

        m_GhostAbility = abilityList[Random.Range(0, abilityList.Count)];

        // Update ability time/cooldown
        PassiveChangeAbilityTime = m_GhostAbility.AbilityTime;
        PassiveChangeAbilityCooldown = PassiveChangeAbilityTime;

        // Resume recovery
        CombatSystem.CombatDataManager.Ghost.ObjectMagic.IsStopRecovery = false;

        // Use the ability
        m_GhostAbility.Use(CombatSystem.CombatDataManager.Ghost);

        // Shake the combat UI
        CombatUI.ShakeUI();
    }

    public void ExitActiveStruggleMode()
    {
        m_CanStartCombatCountdown = false;
        PassiveChangeAbilityCooldown = 0;
        ChangeAbilityRestCooldown = ChangeAbilityRestTime;
        ActiveStruggleMode = false;
        CombatUI.ShowStruggleMode(false);
    }

    public void ExitPassiveStruggleMode()
    {
        ActiveStruggleMode = true;
    }

    public void DoActiveAbility(ActiveAbilityData data)
    {
        CombatSystem.OnActiveAbility?.Invoke(data);
    }
    public void DoPassivebility(PassiveAbilityData data)
    {
        CombatSystem.OnPassiveAbility?.Invoke(data);
    }

    public void ExitCombat()
    {
        m_CanStartCombatCountdown = false;
        m_CanStartCountdown = false;
        ActiveStruggleMode = false;
    }
}
