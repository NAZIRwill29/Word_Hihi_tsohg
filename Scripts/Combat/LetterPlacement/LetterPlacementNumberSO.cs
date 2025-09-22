using UnityEngine;

[CreateAssetMenu(fileName = "LetterPlacementNumberSO", menuName = "CombatSystem/LetterPlacement/LetterPlacementNumberSO")]
public class LetterPlacementNumberSO : LetterPlacementSO
{
    private int m_LetterIndex;

    public override string SetLetterPlacement()
    {
        int index = m_LetterIndex + 1;
        return index.ToString();
    }

    public override bool RewardExorcismLetter(string word, out char value)
    {
        if (m_LetterIndex >= word.Length)
        {
            value = 'A';
            return false;
        }
        char reward = word[m_LetterIndex];
        Debug.Log("reward exorcism letter index(" + m_LetterIndex + ") : " + reward);
        value = reward;
        return true;
    }
}
