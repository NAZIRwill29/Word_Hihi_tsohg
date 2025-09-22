using UnityEngine;

[CreateAssetMenu(fileName = "LetterRuleNumberSO", menuName = "CombatSystem/LetterRule/LetterRuleNumberSO")]
public class LetterRuleNumberSO : LetterRuleSO
{
    [SerializeField] private int m_LetterIndex;

    public override (string, string) SetLetterRule()
    {
        SetRandomLetter();
        int index = m_LetterIndex + 1;
        return (index.ToString(), m_Letter.ToString());
    }

    public override bool CheckLetterRule(string word)
    {
        if (m_LetterIndex >= word.Length)
            return false;
        return word[m_LetterIndex] == m_Letter;
    }
}
