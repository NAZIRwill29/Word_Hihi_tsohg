using System.Collections.Generic;
using UnityEngine;

public class LetterRuleSO : ScriptableObject
{
    [SerializeField]
    protected List<char> m_Letters = new()
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
        'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
    };
    protected char m_Letter;

    public virtual (string, string) SetLetterRule()
    {
        return ("string", "string");
    }

    public virtual bool CheckLetterRule(string word)
    {
        return false;
    }

    protected void SetRandomLetter()
    {
        m_Letter = m_Letters[Random.Range(0, m_Letters.Count)];
    }
}
