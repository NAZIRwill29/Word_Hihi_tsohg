using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordUI : MonoBehaviour
{
    [SerializeField] private WordSystem m_WordSystem;
    [SerializeField] private WordClickSystem m_WordClickSystem;
    [SerializeField] private List<Button> m_WordBtns, m_AlphabetBtns;
    [SerializeField] private List<TextMeshProUGUI> m_WordTexts, m_AlphabetTexts;

    void Awake()
    {
        // Ensure the lists are initialized to avoid null reference errors
        m_WordTexts = new List<TextMeshProUGUI>();
        m_AlphabetTexts = new List<TextMeshProUGUI>();

        foreach (var item in m_WordBtns)  // Fixed: Correctly assigning WordBtns to WordTexts
        {
            m_WordTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        }
        foreach (var item in m_AlphabetBtns)  // Fixed: Correctly assigning AlphabetBtns to AlphabetTexts
        {
            m_AlphabetTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    void Start()
    {
        m_WordClickSystem.MaxWord = m_WordBtns.Count;
        m_WordClickSystem.MaxAlphabet = m_AlphabetBtns.Count;
        UpdateUI();
    }

    public void ChangeLanguage(NameDataFlyweight langNameData)
    {
        m_WordSystem.ChangeLanguage(langNameData);
        UpdateUI();
    }

    public void AddAlphabet()
    {
        m_WordClickSystem.InsertAlphabet();
        UpdateUI();
    }

    public void AlphabetBtnClicked(int num)
    {
        m_WordClickSystem.AlphabetBtnClicked(num);
        UpdateUI();
    }

    public void WordBtnClicked(int num)
    {
        m_WordClickSystem.WordBtnClicked(num);
        UpdateUI();
    }

    public void SuccessWordBtnClicked()
    {
        m_WordClickSystem.ConfirmWord();
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateAlphabetBtnUI();
        UpdateWordBtnUI();
    }

    private void UpdateAlphabetBtnUI()
    {
        foreach (Button alphabetBtn in m_AlphabetBtns)
        {
            alphabetBtn.gameObject.SetActive(false);
        }

        int alphabetLength = Mathf.Min(m_WordClickSystem.AlphabetChars.Length, m_AlphabetBtns.Count);

        for (int i = 0; i < alphabetLength; i++)  // Fixed: Prevent index out-of-range
        {
            m_AlphabetBtns[i].gameObject.SetActive(true);
            m_AlphabetTexts[i].text = m_WordClickSystem.AlphabetChars[i].ToString();
            //Debug.Log("m_AlphabetTexts " + m_WordSystem.AlphabetChars[i]);
        }
    }

    private void UpdateWordBtnUI()
    {
        foreach (Button wordBtn in m_WordBtns)
        {
            wordBtn.gameObject.SetActive(false);
        }

        int wordLength = Mathf.Min(m_WordClickSystem.WordChars.Length, m_WordBtns.Count);

        for (int i = 0; i < wordLength; i++)  // Fixed: Prevent index out-of-range
        {
            m_WordBtns[i].gameObject.SetActive(true);
            m_WordTexts[i].text = m_WordClickSystem.WordChars[i].ToString();
        }
    }
}
