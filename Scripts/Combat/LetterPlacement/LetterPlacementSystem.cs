using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LetterPlacementSystem : MonoBehaviour
{
    [SerializeField] private CombatSystem m_CombatSystem;
    private LetterPlacementSO m_LetterPlacementSO;
    [SerializeField] private WordSystem m_WordSystem;
    public UnityEvent<string> OnSetLetterPlacement;
    public UnityEvent<char> OnRewardLetterPlacement;

    private void OnEnable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessWordNotDuplicate.AddListener(RewardExorcismLetter);
    }

    private void OnDisable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessWordNotDuplicate.RemoveListener(RewardExorcismLetter);
    }

    public void SetRandomLetterPlacement()
    {
        LetterPlacementSO letterPlacementSO = m_CombatSystem.GhostCombatData.LetterPlacementSOs[
            Random.Range(0, m_CombatSystem.GhostCombatData.LetterPlacementSOs.Count)
        ];
        SetLetterPlacement(letterPlacementSO);
    }

    public void SetLetterPlacement(LetterPlacementSO letterPlacementSO)
    {
        m_LetterPlacementSO = letterPlacementSO;
        string letter = m_LetterPlacementSO.SetLetterPlacement();
        OnSetLetterPlacement?.Invoke(letter);
    }

    public void RewardExorcismLetter(string word)
    {
        if (GhostCombatSystem.Instance.ActiveStruggleMode) return;
        if (m_LetterPlacementSO.RewardExorcismLetter(word, out char reward))
            OnRewardLetterPlacement?.Invoke(reward);
    }
}
