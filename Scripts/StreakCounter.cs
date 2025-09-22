using TMPro;
using UnityEngine;

public class StreakCounter : MonoBehaviour
{
    [Tooltip("UI Text element to show streak counter")]
    [SerializeField] private TextMeshProUGUI m_StreakText;

    private int m_CurrentStreak = 0;

    // Properties
    public int CurrentStreak
    {
        get => m_CurrentStreak;
        set
        {
            m_CurrentStreak = value;
            UpdateStreakText();
        }
    }

    private void OnEnable()
    {
        GameEvents.OnCollectibleCollected += IncrementStreak;
    }

    private void OnDisable()
    {
        GameEvents.OnCollectibleCollected -= IncrementStreak;
    }
    private void Start()
    {
        UpdateStreakText();
    }

    // Update the text to show the current streak
    private void UpdateStreakText()
    {
        if (m_StreakText != null)
        {
            m_StreakText.text = m_CurrentStreak.ToString();
        }
    }

    // Increase the streak count
    public void IncrementStreak()
    {
        CurrentStreak++;
    }
}