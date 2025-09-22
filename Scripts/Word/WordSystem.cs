using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WordSystem", menuName = "WordSystem/WordSystem", order = 1)]
public class WordSystem : ScriptableObject
{
    [SerializeField] private AlphabetListManagerSO m_AlphabetListManagerSO;
    [SerializeField] private WordCheck m_WordCheck;
    //[SerializeField] private List<NameDataFlyweight> m_LangNameDatas;
    [SerializeField] private NameDataFlyweight m_LangNameData;

    public UnityEvent<string> OnSuccessWord, OnSuccessWordNotDuplicate, OnFailedWord;
    public UnityEvent<string> OnFailedWordNotDuplicate, OnSuccessExorcismWord, OnFailedExorcismWord;
    public bool isCheckDuplicate = true;

    public void Initialize()
    {
        m_AlphabetListManagerSO.SetAlphabetList(m_LangNameData.Name);
    }

    public void ChangeLanguage(NameDataFlyweight langNameData)
    {
        m_LangNameData = langNameData;
        m_AlphabetListManagerSO.SetAlphabetList(m_LangNameData.Name);
        m_WordCheck.SetupWordList(langNameData.Name);
        m_WordCheck.SetupLetterSets(langNameData.Name);
    }

    public void ConfirmWord(string newWord)
    {
        //Debug.Log("check word : " + newWord);
        string normalizedWord = newWord.Trim().ToUpper();
        if (m_WordCheck.CheckWordExist(normalizedWord))
        {
            Debug.Log("word exist : " + normalizedWord);
            //string word = normalizedWord;
            m_WordCheck.RegisterSuccessWord(normalizedWord);
            OnSuccessWord?.Invoke(normalizedWord);
            if (isCheckDuplicate)
            {
                if (m_WordCheck.CheckSuccessWordDuplicate(normalizedWord, 5))
                    OnSuccessWordNotDuplicate?.Invoke(normalizedWord);
                else
                    OnFailedWordNotDuplicate?.Invoke(normalizedWord);
            }
        }
        else
        {
            Debug.Log("word not exist : " + normalizedWord);
            OnFailedWord?.Invoke(normalizedWord);
        }
    }

    public bool ConfirmExorcismWord(string newWord)
    {
        //Debug.Log("check word : " + newWord);
        string normalizedWord = newWord.Trim().ToUpper();
        if (m_WordCheck.CheckWordExist(normalizedWord))
        {
            Debug.Log("word exist : " + normalizedWord);
            //string word = normalizedWord;
            m_WordCheck.RegisterSuccessExorcismWord(normalizedWord);
            OnSuccessExorcismWord?.Invoke(normalizedWord);
            return true;
        }
        else
        {
            Debug.Log("word not exist : " + normalizedWord);
            OnFailedExorcismWord?.Invoke(normalizedWord);
            return false;
        }
    }

    public char GetRandomAlphabet()
    {
        List<char> alphabetList = m_AlphabetListManagerSO.CurrentAlphabetListSO.GetListAlphabet();
        char randomLetter = alphabetList[Random.Range(0, alphabetList.Count)];
        //Debug.Log("Random Alphabet: " + randomLetter);
        return randomLetter;
    }

    public List<char> GetUnduplicatedRandomAlphabets(int count)
    {
        List<char> alphabetList = new List<char>(m_AlphabetListManagerSO.CurrentAlphabetListSO.GetListAlphabet());

        // Safety check
        if (count > alphabetList.Count)
            count = alphabetList.Count;

        // Shuffle
        for (int i = 0; i < alphabetList.Count; i++)
        {
            int randomIndex = Random.Range(i, alphabetList.Count);
            (alphabetList[i], alphabetList[randomIndex]) = (alphabetList[randomIndex], alphabetList[i]);
        }

        return alphabetList.GetRange(0, count);
    }

}
