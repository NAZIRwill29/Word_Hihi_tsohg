using UnityEngine;

[CreateAssetMenu(fileName = "LetterRuleNonNumberFirstSO", menuName = "CombatSystem/LetterRule/LetterRuleNonNumberFirstSO")]
public class LetterRuleNonNumberFirstSO : LetterRuleNonNumberSO
{
    public override (string, string) SetLetterRule()
    {
        SetRandomLetter();
        return ("First", m_Letter.ToString());
    }

    public override bool CheckLetterRule(string word)
    {
        return word[0] == m_Letter;
    }
}
