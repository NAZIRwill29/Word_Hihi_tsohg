using UnityEngine;

public class LetterPlacementSO : ScriptableObject
{
    public virtual string SetLetterPlacement()
    {
        return " ";
    }
    public virtual bool RewardExorcismLetter(string word, out char value)
    {
        value = 'A';
        return true;
    }
}
