using UnityEngine;

[CreateAssetMenu(fileName = "ShroudStruggleModeGhostAbility", menuName = "Abilities/Ghost/ShroudStruggleModeGhostAbility")]
public class ShroudStruggleModeGhostAbility : ActiveGhostAbility
{
    [SerializeField] private WordSystem m_WordSystem;
    private float m_PlayerMeter;
    [SerializeField] private float m_AddNum = 0.1f;
    [SerializeField] private float m_MinusNum = 0.01f;
    [SerializeField] private float m_MinusTime = 1f;
    private float m_MinusCooldown;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_PlayerMeter = 0.5f;
        m_MinusCooldown = m_MinusTime;

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.ShowFightMeter(true);
        m_WordSystem.isCheckDuplicate = false;
    }

    public override void DoUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        base.DoUpdate();
        m_MinusCooldown -= Time.deltaTime;
        if (m_MinusCooldown <= 0)
        {
            m_MinusCooldown = m_MinusTime;
            m_PlayerMeter -= m_MinusNum;
            GhostCombatSystem.Instance.CombatUI.ChangeFightMeter(m_PlayerMeter);
        }
        if (m_PlayerMeter <= 0)
            DoPunishment();
    }

    public override void OnSuccessWord(string word)
    {
        if (word == m_Words[m_Index])
        {
            m_PlayerMeter += m_AddNum;
            GhostCombatSystem.Instance.CombatUI.ChangeFightMeter(m_PlayerMeter);
            if (m_PlayerMeter >= 1)
                DoReward();
        }
        else
            PrevChallenge();
    }

    public override void OnFailedWord(string word)
    {
        PrevChallenge();
    }

    private void PrevChallenge()
    {
        m_PlayerMeter -= m_AddNum;
        GhostCombatSystem.Instance.CombatUI.ChangeFightMeter(m_PlayerMeter);

        m_Words[m_Index] = GhostCombatSystem.Instance.WordCheck.GetRandomWord(m_WordLengthMin, m_WordLengthMax);

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Words.Clear();
        m_PlayerMeter = 0.5f;
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(string.Empty);
        GhostCombatSystem.Instance.CombatUI.ShowFightMeter(false);
        m_WordSystem.isCheckDuplicate = true;
    }
}
