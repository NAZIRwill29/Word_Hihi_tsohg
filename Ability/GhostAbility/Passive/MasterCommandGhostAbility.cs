using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterCommandGhostAbility", menuName = "Abilities/Ghost/MasterCommandGhostAbility")]
public class MasterCommandGhostAbility : PassiveGhostAbility
{
    private List<char> m_Letters = new();
    [SerializeField] protected HealthData m_ExtraHealthData;
    [SerializeField] private Color m_TextColor = Color.red;//6
    // [SerializeField] private float m_MinusTime = -0.2f;
    private string m_PrevWord;
    [SerializeField] private MagicData healMagicData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_Letters.Clear();
        m_PrevWord = string.Empty;

        //get letter from text in PassiveAbilityData
        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            m_Letters.Add(m_Words[m_Index][i]);
        }

        GhostCombatSystem.Instance.CombatUI.ChangeBadScrambleTexts(m_Letters, 0.8f, m_TextColor);
        GhostCombatSystem.Instance.CombatUI.SetRandomPositionBadScrambleTexts(m_Letters.Count);

        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            GhostCombatSystem.Instance.CombatUI.SetBadScrambleTextFlicker(i, 1);
        }
        GhostCombatSystem.Instance.CombatUI.SetSrambleTextFlickerTime(m_Letters.Count, 0.5f);
    }

    public override void OnTyping(string word)
    {
        if (string.IsNullOrEmpty(word) || word.Length <= 0) return;
        //Debug.Log("OnCompleteWord ability : " + word);
        Debug.Log("m_PrevWord : " + m_PrevWord + " | word : " + word);
        //init when not used backspace
        if (m_PrevWord.Length < word.Length)
        {
            if (WordChecking.CheckContainLetter(m_Letters, word[^1].ToString()))
                DoPunishment();
        }

        m_PrevWord = word;
    }

    public override void OnSuccessWord(string word)
    {
        m_PrevWord = string.Empty;
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("ShadowyFigureTrig");

        int random = Random.Range(0, 4);
        if (random == 0)
        {
            GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_ExtraHealthData);
            GhostCombatSystem.Instance.CombatUI.ShakePlayer();
        }
        else
        {
            //INCREASE GHOST mana
            GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectMagic.Heal(healMagicData);
        }
    }

    public override void ExitAbility()
    {
        m_Letters.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeBadScrambleTexts(m_Letters, 0);
        GhostCombatSystem.Instance.CombatUI.ReseFlickerUI();
    }
}
