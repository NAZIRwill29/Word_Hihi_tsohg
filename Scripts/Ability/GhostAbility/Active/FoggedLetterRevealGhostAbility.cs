using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoggedLetterRevealGhostAbility", menuName = "Abilities/Ghost/FoggedLetterRevealGhostAbility")]
public class FoggedLetterRevealGhostAbility : ActiveGhostAbility
{
    private List<char> m_Letters = new();
    private List<char> m_RemoveLetters = new();
    private List<int> m_Indexs = new();
    private List<int> m_RemoveIndexs = new();
    private bool m_HasWarning;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_RemoveIndexs.Clear();
        m_RemoveLetters.Clear();
        m_Letters.Clear();
        m_Indexs.Clear();
        m_HasWarning = false;

        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            m_Letters.Add(m_Words[m_Index][i]);
        }

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBlocks(true);
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextBlockAnimation("Fogged", true);

        for (int i = 0; i < GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextBlockMax; i++)
        {
            m_Indexs.Add(i);
        }
        ShowObstacle(0, false);
        int num = Random.Range(0, m_Indexs.Count - 1);
        ShowObstacle(num, false);
    }

    public override void OnSuccessWord(string word)
    {
        if (m_Indexs.Count == 0)
        {
            Debug.Log("last Challenge");
            if (word == m_Words[m_Index])
                DoReward();
            else
                GhostCombatSystem.Instance.CombatUI.TypingPromptContShow("FlickerTrig", "I want my word!!!");
            return;
        }

        if (WordChecking.TryGetFirstMatchingLetter(m_Letters, word, out char letter))
        {
            Debug.Log("NextChallenge");
            NextChallenge();
            m_Letters.Remove(letter);
            m_RemoveLetters.Add(letter);
        }
        else
        {
            if (WordChecking.CheckContainLetter(m_RemoveLetters, word))
            {
                GhostCombatSystem.Instance.CombatUI.TypingPromptContShow("FlickerTrig", "previous letter cannot be used again");
                m_HasWarning = true;
                if (m_HasWarning)
                    PrevChallenge();
            }
            else
            {
                Debug.Log("no letter same");
                PrevChallenge();
            }
        }
    }

    private void PrevChallenge()
    {
        if (m_RemoveIndexs.Count == 0) return;

        int num = m_RemoveIndexs[Random.Range(0, m_RemoveIndexs.Count)];
        ShowObstacle(num, true);
    }

    protected override void NextChallenge()
    {
        if (m_Indexs.Count == 0) return;

        int num = m_Indexs[Random.Range(0, m_Indexs.Count)];
        ShowObstacle(num, false);
    }

    private void ShowObstacle(int num, bool isTrue)
    {
        if (!isTrue)
        {
            m_RemoveIndexs.Add(num);
            if (m_Indexs.Exists(x => x == num))
                m_Indexs.Remove(num);
        }
        else
        {
            m_Indexs.Add(num);
            if (m_RemoveIndexs.Exists(x => x == num))
                m_RemoveIndexs.Remove(num);
        }
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBlock(num, isTrue);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Letters.Clear();
        m_Indexs.Clear();
        m_RemoveIndexs.Clear();
        m_RemoveLetters.Clear();

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigText("");
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBlocks(false);
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextCircleAnimation("Flicker", false);
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextBlockAnimation("Fogged", false);
    }
}