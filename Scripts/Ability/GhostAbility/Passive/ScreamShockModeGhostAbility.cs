using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ScreamShockModeGhostAbility", menuName = "Abilities/Ghost/ScreamShockModeGhostAbility")]
public class ScreamShockModeGhostAbility : PassiveGhostAbility
{
    [SerializeField] private float m_ScreamTimeMin = 2.5f, m_ScreamTimeMax = 5;
    private float m_ScreamCooldown;
    [SerializeField] private ThingHappenData m_ThingHappenData = new();
    [SerializeField] private HealthData m_HealthData;
    private Tween m_ChangeTypingWordTween;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_ScreamCooldown = Random.Range(m_ScreamTimeMin, m_ScreamTimeMax);
    }

    public override void DoUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        m_ScreamCooldown -= Time.deltaTime;
        if (m_ScreamCooldown <= 0)
        {
            GhostCombatSystem.Instance.CombatSystem.GhostT.ThingHappen(m_ThingHappenData);
            GhostCombatSystem.Instance.CombatUI.ShakeTypingUI();
            m_ChangeTypingWordTween?.Kill();
            m_ChangeTypingWordTween = DOVirtual
                .DelayedCall(0.4f, ChangeTypingWord)
                .SetId(this);  // namespace it so we can kill all if needed
            m_ScreamCooldown = Random.Range(m_ScreamTimeMin, m_ScreamTimeMax);
        }
    }

    public override void OnFailedWord(string word)
    {
        DoPunishment();
    }

    public void ChangeTypingWord()
    {
        int num = Random.Range(1, 3);
        GhostCombatSystem.Instance.CombatSystem.CombatTypingSystem.ChangeLetterInTypingWordRandomly(num);
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_HealthData);
        GhostCombatSystem.Instance.CombatUI.ShakePlayer();
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_ScreamCooldown = Random.Range(m_ScreamTimeMin, m_ScreamTimeMax);
    }
}
