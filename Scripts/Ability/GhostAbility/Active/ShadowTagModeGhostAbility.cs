using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowTagModeGhostAbility", menuName = "Abilities/Ghost/ShadowTagModeGhostAbility")]
public class ShadowTagModeGhostAbility : ActiveGhostAbility
{
    private List<string> m_TempWords = new();
    [SerializeField] private float m_ChangeTimeMin = 3f, m_ChangeTimeMax = 6f;
    private float m_ChangeCooldown, m_ChangeTime;
    [SerializeField] private StabilityData m_StabilityData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_TempWords.Clear();
        m_ChangeTime = Random.Range(m_ChangeTimeMin, m_ChangeTimeMax);
        m_ChangeCooldown = m_ChangeTime;

        foreach (string word in m_Words)
        {
            m_TempWords.Add(word);
        }

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeTexts(m_TempWords);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.ChangeIsCheckWordNotExist(false);
    }

    public override void DoUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        base.DoUpdate();

        m_ChangeCooldown -= Time.deltaTime;
        if (m_ChangeCooldown <= 0)
        {
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("ShadowyFigureTrig");
            m_ChangeTime = Random.Range(m_ChangeTimeMin, m_ChangeTimeMax);
            m_ChangeCooldown = m_ChangeTime;

            for (int i = 0; i < m_TempWords.Count; i++)
            {
                m_TempWords[i] = WordEffectUtils.ScrambleString(m_TempWords[i]);
            }

            GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeTexts(m_TempWords);
            GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
        }
    }

    public override void OnCompleteWord(string word)
    {
        if (WordChecking.CheckIsWordExistInList(m_TempWords, word))
        {
            NextChallenge(word);
        }
        else
        {
            PrevChallenge();
        }
    }

    protected void NextChallenge(string word)
    {
        Debug.Log("NextChallenge");
        m_TempWords.Remove(word);
        base.NextChallenge();
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeTexts(m_TempWords);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
    }

    private void PrevChallenge()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectStability.TakeDamage(m_StabilityData);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Words.Clear();
        m_TempWords.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeTexts(m_Words);
        GhostCombatSystem.Instance.CombatUI.ChangeIsCheckWordNotExist(true);
    }
}
