using UnityEngine;

[CreateAssetMenu(fileName = "LetterRuleNonNumberLastSO", menuName = "CombatSystem/LetterRule/LetterRuleNonNumberLastSO")]
public class LetterRuleNonNumberLastSO : LetterRuleNonNumberSO
{
    public override (string, string) SetLetterRule()
    {
        SetRandomLetter();
        return ("Last", m_Letter.ToString());
    }

    public override bool CheckLetterRule(string word)
    {
        return word[word.Length - 1] == m_Letter;
    }
}
