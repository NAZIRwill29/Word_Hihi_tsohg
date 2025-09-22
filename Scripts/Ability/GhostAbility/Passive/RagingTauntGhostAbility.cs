using UnityEngine;

[CreateAssetMenu(fileName = "RagingTauntGhostAbility", menuName = "Abilities/Ghost/RagingTauntGhostAbility")]
public class RagingTauntGhostAbility : PassiveGhostAbility
{
    [SerializeField] private StabilityData m_DamageStabilityData;
    [SerializeField] private StabilityData m_HealStabilityData;
    [SerializeField] private float m_DrainTime;
    private string m_TypingWord;
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_TypingWord = string.Empty;
        GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_Words, true);
        GhostCombatSystem.Instance.CombatSystem.GhostStabilityStatDrainOverTime.DrainTime = m_DrainTime;
    }

    public override void OnTyping(string word)
    {
        //check backspace usage
        if (!string.IsNullOrEmpty(word) && m_TypingWord.Length > word.Length)
        {
            DoPunishment();
        }

        m_TypingWord = word;
    }

    public override void OnSuccessWord(string word)
    {
        if (WordChecking.CheckIsWordExistInList(m_Words, word))
        {
            DoReward();
            m_Words.Remove(word);
            GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_Words, true);
        }
        // else
        //     DoPunishment();

        m_TypingWord = string.Empty;
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectStability.TakeDamage(m_DamageStabilityData);
    }

    public override void DoReward()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectStability.Heal(m_HealStabilityData);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Words.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_Words, false);
        GhostCombatSystem.Instance.CombatSystem.GhostStabilityStatDrainOverTime.ResetDrainCooldown();
    }
}
