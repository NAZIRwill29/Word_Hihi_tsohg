using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodDrainMistakeGhostAbility", menuName = "Abilities/Ghost/BloodDrainMistakeGhostAbility")]
public class BloodDrainMistakeGhostAbility : PassiveGhostAbility
{
    private string m_PrevWord;
    [SerializeField] protected HealthData m_healthData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_PrevWord = string.Empty;
    }

    public override void OnTyping(string word)
    {
        Debug.Log("m_PrevWord : " + m_PrevWord + " | word : " + word);
        if (string.IsNullOrEmpty(word) || word.Length <= 0) return;
        //check if has used backspace - init when used backspace
        if (m_PrevWord.Length > word.Length)
        {
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("DrippingBlood");
            DoPunishment();
        }

        m_PrevWord = word;
    }

    public override void OnSuccessWord(string word)
    {
        m_PrevWord = string.Empty;
    }

    public override void OnFailedWord(string word)
    {
        if (GhostCombatSystem.Instance.CombatUI)
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("DrippingBlood");

        DoPunishment();
    }

    public override void DoPunishment()
    {
        Debug.Log("DoPunishment");
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_healthData);
        GhostCombatSystem.Instance.CombatUI.ShakePlayer();
    }
}
