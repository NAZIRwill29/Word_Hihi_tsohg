using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EscapeWordSystem : MonoBehaviour
{
    [SerializeField] private CombatSystem m_CombatSystem;
    [SerializeField] private WordSystem m_WordSystem;
    [SerializeField] private ListStringDataFlyweight m_EscapeWordDatas;
    private string m_EscapeWord;
    public UnityEvent<string> OnSetEscapeWord;
    public UnityEvent OnEscapeWordSuccess, OnEscapeWordFailed;

    private void OnEnable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessExorcismWord.AddListener(CheckEscapeWord);
    }

    private void OnDisable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessExorcismWord.RemoveListener(CheckEscapeWord);
    }

    public void StartCombat()
    {
        SetEscapeWord();
    }

    public void SetEscapeWord()
    {
        m_EscapeWord = m_EscapeWordDatas.Strings[Random.Range(0, m_EscapeWordDatas.Strings.Count)];
        OnSetEscapeWord?.Invoke(m_EscapeWord);
    }

    public void CheckEscapeWord(string word)
    {
        // if (GhostCombatSystem.Instance.ActiveStruggleMode)
        // {
        //     return;
        // }
        if (m_CombatSystem.CombatDataManager.GhostTemplate.EscapeNum <= 0)
        {
            Debug.Log("escape word failed");
            OnEscapeWordFailed?.Invoke();
            m_CombatSystem.CombatUI.ShowAbilityPrompt2(true, "Can't escape anymore");
        }
        if (word == m_EscapeWord)
        {
            Debug.Log("escape word succes");
            OnEscapeWordSuccess?.Invoke();
        }
    }
}
