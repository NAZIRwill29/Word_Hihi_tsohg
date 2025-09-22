using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostAbility", menuName = "Abilities/Ghost/GhostAbility")]
public class GhostAbility : Ability
{
    public float AbilityTime;
    [SerializeField] protected int m_WordCount = 3;
    [SerializeField] protected int m_WordLengthMin = 8, m_WordLengthMax = 13;
    [SerializeField] protected List<string> m_Words = new();
    protected int m_Index;


    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        // Use method logs name, plays sound, and particle effect
        Debug.Log($"Using Ghost ABility : {AbilityName}");
        // if (objectT is Character character)
        //     character.ObjectDefense.Defense(AbilityName);
        ThingHappen(objectT);
        m_Words.Clear();
        m_Index = 0;
        m_Words = GhostCombatSystem.Instance.WordCheck.GetRandomWords(m_WordLengthMin, m_WordLengthMax, m_WordCount);
    }

    public virtual void DoUpdate()
    {
    }

    public virtual void DoPunishment()
    {
    }

    public virtual void DoReward()
    {
    }

    public virtual void OnTyping(string word)
    {
    }

    public virtual void OnChangeExorcismLetter(List<char> letters)
    {
    }

    public virtual void OnExorcismTyping(string word)
    {
    }

    public virtual void OnCompleteWord(string word)
    {
    }

    public virtual void OnSuccessWord(string word)
    {
    }

    public virtual void OnFailedWord(string word)
    {
    }

    public virtual void OnSuccessWordNotDuplicate(string word)
    {
    }

    public virtual void OnFailedWordNotDuplicate(string word)
    {
    }

    public virtual void OnSuccesLetterRule()
    {
    }

    public virtual void OnFailedLetterRule()
    {
    }

    public virtual void OnSuccessExorcismWord(string word)
    {
    }

    public virtual void OnFailedExorcismWord(string word)
    {
    }

    public virtual void OnExorciseWeaknessWord(string word)
    {
    }

    public virtual void OnExorciseCurseWord(string word)
    {
    }

    public virtual void OnFlickerStartBright(int num)
    {
    }

    public virtual void OnUseInCannotUseExorcismLetter()
    {
    }

    public virtual void ExitAbility()
    {
    }
}
