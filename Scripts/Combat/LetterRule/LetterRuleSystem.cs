using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LetterRuleSystem : MonoBehaviour
{
    [SerializeField] private CombatSystem m_CombatSystem;
    private LetterRuleSO m_LetterRuleSO;
    [SerializeField] private WordSystem m_WordSystem;
    public UnityEvent<string, string> OnSetLetterRule;
    public UnityEvent OnSuccesLetterRule, OnFailedLetterRule;

    private void OnEnable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessWordNotDuplicate.AddListener(CheckLetterRule);
    }

    private void OnDisable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessWordNotDuplicate.RemoveListener(CheckLetterRule);
    }

    public void SetRandomLetterRule()
    {
        LetterRuleSO letterRuleSO = m_CombatSystem.GhostCombatData.LetterRuleSOs[
            Random.Range(0, m_CombatSystem.GhostCombatData.LetterRuleSOs.Count)
        ];
        SetLetterRule(letterRuleSO);
    }

    public void SetLetterRule(LetterRuleSO letterRuleSO)
    {
        m_LetterRuleSO = letterRuleSO;
        (string index, string letter) = m_LetterRuleSO.SetLetterRule();
        OnSetLetterRule?.Invoke(index, letter);
    }

    public void CheckLetterRule(string word)
    {
        if (GhostCombatSystem.Instance.ActiveStruggleMode) return;
        if (m_LetterRuleSO.CheckLetterRule(word))
        {
            Debug.Log("letter rule success");
            OnSuccesLetterRule?.Invoke();
        }
        else
        {
            Debug.Log("letter rule failed");
            OnFailedLetterRule?.Invoke();
        }
    }
}
