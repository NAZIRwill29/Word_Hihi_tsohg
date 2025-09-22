using UnityEngine;

[CreateAssetMenu(fileName = "WordScrambleEscapeGhostAbility", menuName = "Abilities/Ghost/WordScrambleEscapeGhostAbility")]
public class WordScrambleEscapeGhostAbility : ActiveGhostAbility
{
    private string m_Scramble;
    private int m_RevealCount = 0;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);

        m_Scramble = WordEffectUtils.ScrambleString(m_Words[m_Index]);
        m_RevealCount = 0;

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Scramble);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
    }

    public override void OnSuccessWord(string word)
    {
        if (word.Length == 0 || word.Length != m_Words[m_Index].Length) return;

        if (word == m_Words[m_Index])
        {
            DoReward();
        }
        else
        {
            int maxReveal = Mathf.CeilToInt(m_Scramble.Length / 2f);
            if (m_RevealCount >= maxReveal) return;

            char[] scrambleChars = m_Scramble.ToCharArray();
            string target = m_Words[m_Index];

            // Find next incorrect letter from the left
            for (int i = m_RevealCount; i < scrambleChars.Length; i++)
            {
                if (scrambleChars[i] != target[i])
                {
                    // Find the correct letter somewhere else in the scramble
                    for (int j = i + 1; j < scrambleChars.Length; j++)
                    {
                        if (scrambleChars[j] == target[i] && target[j] != scrambleChars[j])
                        {
                            // Swap
                            (scrambleChars[i], scrambleChars[j]) = (scrambleChars[j], scrambleChars[i]);
                            m_Scramble = new string(scrambleChars);
                            m_RevealCount++;
                            GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Scramble);
                            GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
                            return;
                        }
                    }
                }
            }
        }
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Words.Clear();
        m_RevealCount = 0;
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(string.Empty);
    }
}
