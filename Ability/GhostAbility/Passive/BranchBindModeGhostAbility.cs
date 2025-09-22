using UnityEngine;
/// <summary>
/// Fill in()
/// -ExorcismLetterCoverSprite
/// </summary>
[CreateAssetMenu(fileName = "BranchBindModeGhostAbility", menuName = "Abilities/Ghost/BranchBindModeGhostAbility")]
public class BranchBindModeGhostAbility : PassiveGhostAbility
{
    private int m_ExorcismLetterNum;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_ExorcismLetterNum = 0;
    }

    public override void OnFailedWord(string word)
    {
        DoPunishment();
    }

    public override void OnFailedLetterRule()
    {
        DoPunishment();
    }

    public override void OnSuccesLetterRule()
    {
        DoReward();
    }

    public override void DoPunishment()
    {
        m_ExorcismLetterNum--;
        if (m_ExorcismLetterNum < 0)
            m_ExorcismLetterNum = 0;
        GhostCombatSystem.Instance.CombatUI.ShowExorcismLetterCoverImage(m_PassiveAbilityData.ExorcismLetterCoverSprites[m_ExorcismLetterNum]);
    }

    public override void DoReward()
    {
        m_ExorcismLetterNum++;
        if (m_ExorcismLetterNum > m_PassiveAbilityData.ExorcismLetterCoverSprites.Count)
            m_ExorcismLetterNum = m_PassiveAbilityData.ExorcismLetterCoverSprites.Count - 1;
        GhostCombatSystem.Instance.CombatUI.ShowExorcismLetterCoverImage(m_PassiveAbilityData.ExorcismLetterCoverSprites[m_ExorcismLetterNum]);
    }

    public override void ExitAbility()
    {
        GhostCombatSystem.Instance.CombatUI.ShowExorcismLetterCoverImage(null);
    }
}
