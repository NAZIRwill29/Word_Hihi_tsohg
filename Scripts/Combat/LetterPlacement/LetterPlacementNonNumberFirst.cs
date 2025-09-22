using UnityEngine;

[CreateAssetMenu(fileName = "LetterPlacementNonNumberFirst", menuName = "CombatSystem/LetterPlacement/LetterPlacementNonNumberFirst")]
public class LetterPlacementNonNumberFirst : LetterPlacementNonNumberSO
{
    public override string SetLetterPlacement()
    {
        return "First";
    }

    public override bool RewardExorcismLetter(string word, out char value)
    {
        char reward = word[0];
        Debug.Log("reward exorcism letter first : " + reward);
        value = reward;
        return true;
    }
}
