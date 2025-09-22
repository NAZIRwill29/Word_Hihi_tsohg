using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WordClickSystem", menuName = "WordSystem/WordClickSystem")]
public class WordClickSystem : ScriptableObject
{
    [SerializeField] private WordSystem m_WordSystem;
    public string WordChars { get; private set; }
    public string AlphabetChars { get; private set; }
    public int MaxWord { get; set; }
    public int MaxAlphabet { get; set; }
    public UnityEvent OnAddAlphabet, OnAlphabet, OnWord;

    public void AlphabetBtnClicked(int num)
    {
        InsertWord(AlphabetChars[num], num);
    }

    public void WordBtnClicked(int num)
    {
        InsertAlphabet(WordChars[num], num);
    }

    public void InsertWord(char abc, int num)
    {
        if (WordChars.Length >= MaxWord)
        {
            Debug.Log("m_WordChars has maxed");
            return;
        }
        AlphabetChars = RemoveCharAtIndex(AlphabetChars, num);
        WordChars += abc;
        OnAlphabet?.Invoke();
    }

    public void InsertAlphabet(char abc = '\0', int num = 0)
    {
        if (AlphabetChars.Length >= MaxAlphabet)
        {
            Debug.Log("m_AlphabetChars has reached its maximum length.");
            return;
        }

        if (abc == '\0')
        {
            AlphabetChars += m_WordSystem.GetRandomAlphabet();
            OnAddAlphabet?.Invoke();
        }
        else
        {
            WordChars = RemoveCharAtIndex(WordChars, num);
            AlphabetChars += abc;
            OnWord?.Invoke();
        }
    }

    public string RemoveCharAtIndex(string input, int index)
    {
        if (index < 0 || index >= input.Length)
        {
            Debug.LogError("Index is out of range!");
            return input;
        }

        return input.Remove(index, 1);
    }

    public void ConfirmWord()
    {
        m_WordSystem.ConfirmWord(WordChars);
        WordChars = string.Empty;
    }
}
