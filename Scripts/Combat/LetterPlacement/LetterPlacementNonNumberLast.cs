using UnityEngine;

[CreateAssetMenu(fileName = "LetterPlacementNonNumberLast", menuName = "CombatSystem/LetterPlacement/LetterPlacementNonNumberLast")]
public class LetterPlacementNonNumberLast : LetterPlacementNonNumberSO
{
    public override string SetLetterPlacement()
    {
        return "Last";
    }

    public override bool RewardExorcismLetter(string word, out char value)
    {
        char reward = word[word.Length - 1];
        Debug.Log("reward exorcism letter last : " + reward);
        value = reward;
        return true;
    }
}
