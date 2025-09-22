using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordLanguageDropdownUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    // Reference to your actual language-changing system
    [SerializeField] private WordSystem m_WordSystem;

    // Replace with your actual language codes/names
    [SerializeField] private List<NameDataFlyweight> m_SupportedLanguages;

    private void Start()
    {
        SetupDropdown();
    }

    private void SetupDropdown()
    {
        dropdown.ClearOptions();

        // You can also show full language names if you want
        List<string> displayNames = new();
        foreach (var item in m_SupportedLanguages)
        {
            displayNames.Add(item.Name);
        }
        dropdown.AddOptions(displayNames);

        dropdown.onValueChanged.AddListener(OnLanguageSelected);
    }

    private void OnLanguageSelected(int index)
    {
        //string selectedLangCode = m_SupportedLanguages[index].Name;
        // Call your actual function to change the language
        m_WordSystem.ChangeLanguage(m_SupportedLanguages[index]);
    }
}
