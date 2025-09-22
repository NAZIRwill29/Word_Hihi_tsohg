using UnityEngine;

public class CombatStarter : MonoBehaviour
{
    [SerializeField] protected CombatDataManager m_CombatDataManager;
    [SerializeField] private float m_CooldownDuration = 20;
    private float m_CooldownTime;

    void Update()
    {
        if (m_CooldownTime > 0)
            m_CooldownTime -= Time.deltaTime;
    }

    public void StartCombat(Ghost ghost)
    {
        if (m_CooldownTime > 0) return;
        if (GameManager.Instance.PlayModeManager.PlayModeCurrentName == "Combat") return;

        m_CombatDataManager.Ghost = ghost;
        m_CombatDataManager.GhostCombatDataFlyweight = ghost.GhostCombatDataFlyweight;
        m_CombatDataManager.GhostTemplate = ghost.GhostTemplate;

        GameManager.Instance.ChangePlayMode("Combat");

        GameManager.Instance.MiniGameManager.CanMiniGame = false;
        GameManager.Instance.MiniGameManager.ExitMiniGame(true);
        //GameManager.Instance.HidingMechanic.UnHide();

        m_CombatDataManager.Ghost.IsCanExecute = false;
    }

    public void StartCooldown()
    {
        m_CooldownTime = m_CooldownDuration;
    }
}
