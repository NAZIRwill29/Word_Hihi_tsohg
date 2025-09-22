using UnityEngine;
using UnityEngine.Events;
//NOT NEEDED()
public class CombatDistanceSystem : MonoBehaviour
{
    // [SerializeField] private CombatSystem m_CombatSystem;
    // private GhostTemplate m_GhostTemplate;
    // private float m_DistancePercentChangePlus;
    // private float m_DistancePercent;
    // private float m_DistancePercentChangeFactor = 1;
    // private float m_MaxDistancePercent = 100f;
    // private float m_GhostMoveCooldown;
    // private float m_GhostMoveTime;
    // [SerializeField, ReadOnly] private float m_GhostMoveAllTime;
    // private float m_GhostMoveChangeCooldown;
    // private bool m_GhostMoveChangeDueAbility;
    // [SerializeField] private float m_GhostMoveChangeTimeOri = 15;
    // private float m_GhostMoveChangeTime;
    // [SerializeField, Range(50, 100)] float m_SuccessStruggelModePercentPush = 80;
    // [SerializeField, Range(0, 50)] float m_FailedStruggelModePercentPush = 40;
    // public UnityEvent<float> OnGhostMove;
    // public UnityEvent<float> OnGhostMovePlus;
    // public UnityEvent<float> OnGhostSpeedIncrease;
    // public UnityEvent OnReachPlayer;
    // private bool m_GhostReachPlayer;

    // private void OnEnable()
    // {
    //     if (!m_CombatSystem) return;

    //     m_CombatSystem.OnSuccessFacingStruggleMode.AddListener(OnSuccessFacingStruggleMode);
    //     m_CombatSystem.OnFailedFacingStruggleMode.AddListener(OnFailedFacingStruggleMode);

    //     if (m_CombatSystem.LetterRuleSystem != null)
    //     {
    //         m_CombatSystem.LetterRuleSystem.OnSuccesLetterRule.AddListener(OnSuccesLetterRule);
    //         m_CombatSystem.LetterRuleSystem.OnFailedLetterRule.AddListener(OnFailedLetterRule);
    //     }

    //     if (m_CombatSystem.GhostStabilityStateManager)
    //     {
    //         m_CombatSystem.GhostStabilityStateManager.OnLowStability.AddListener(OnLowStability);
    //         m_CombatSystem.GhostStabilityStateManager.OnHighStability.AddListener(OnHighStability);
    //     }
    // }

    // private void OnDisable()
    // {
    //     if (!m_CombatSystem) return;

    //     m_CombatSystem.OnSuccessFacingStruggleMode.RemoveListener(OnSuccessFacingStruggleMode);
    //     m_CombatSystem.OnFailedFacingStruggleMode.RemoveListener(OnFailedFacingStruggleMode);

    //     if (m_CombatSystem.LetterRuleSystem != null)
    //     {
    //         m_CombatSystem.LetterRuleSystem.OnSuccesLetterRule.RemoveListener(OnSuccesLetterRule);
    //         m_CombatSystem.LetterRuleSystem.OnFailedLetterRule.RemoveListener(OnFailedLetterRule);
    //     }

    //     if (m_CombatSystem.GhostStabilityStateManager)
    //     {
    //         m_CombatSystem.GhostStabilityStateManager.OnLowStability.RemoveListener(OnLowStability);
    //         m_CombatSystem.GhostStabilityStateManager.OnHighStability.RemoveListener(OnHighStability);
    //     }
    // }

    // void Update()
    // {
    //     if (GameManager.Instance.IsPause) return;
    //     if (!m_CombatSystem.IsStart) return;
    //     if (m_CombatSystem.IsEnd) return;
    //     if (m_GhostReachPlayer || m_DistancePercent <= 0f)
    //         return;

    //     float delta = Time.deltaTime;

    //     m_GhostMoveCooldown -= delta;
    //     if (m_GhostMoveCooldown <= 0f)
    //     {
    //         m_GhostMoveCooldown = m_GhostMoveAllTime;

    //         m_DistancePercent += m_GhostTemplate.DistancePercentChange * m_DistancePercentChangeFactor + m_DistancePercentChangePlus;
    //         m_DistancePercent = Mathf.Clamp(m_DistancePercent, 0f, m_MaxDistancePercent);

    //         OnGhostMove?.Invoke(m_DistancePercent);
    //         m_DistancePercentChangePlus = 0f;

    //         if (m_DistancePercent <= 0f)
    //         {
    //             Debug.Log("Ghost reached player!");
    //             GhostReachedPlayer();
    //         }
    //     }

    //     if (!m_GhostMoveChangeDueAbility) return;

    //     m_GhostMoveChangeCooldown -= delta;
    //     if (m_GhostMoveChangeCooldown <= 0)
    //     {
    //         m_GhostMoveChangeCooldown = m_GhostMoveChangeTime;
    //         ResetGhostMoveTime();
    //     }
    // }

    // public void StartCombat(GhostTemplate ghostTemplate)
    // {
    //     m_GhostTemplate = ghostTemplate;
    //     m_DistancePercent = m_MaxDistancePercent;
    //     m_GhostMoveTime = m_GhostTemplate.GhostMoveTime;
    //     m_GhostMoveAllTime = m_GhostMoveTime;
    //     m_GhostMoveCooldown = m_GhostMoveAllTime;
    //     m_GhostReachPlayer = false;
    //     m_GhostMoveChangeTime = m_GhostMoveChangeTimeOri;
    //     m_GhostMoveChangeCooldown = m_GhostMoveChangeTime;
    //     m_DistancePercentChangeFactor = 1;
    // }

    // private void OnLowStability(string name)
    // {
    //     m_DistancePercentChangeFactor = 2;
    // }

    // private void OnHighStability(string name)
    // {
    //     m_DistancePercentChangeFactor = 1;
    // }

    // private void OnSuccessFacingStruggleMode()
    // {
    //     if (!GhostCombatSystem.Instance.ActiveStruggleMode) return;
    //     DistanceChangePlus(m_SuccessStruggelModePercentPush);
    // }

    // private void OnFailedFacingStruggleMode()
    // {
    //     if (!GhostCombatSystem.Instance.ActiveStruggleMode) return;
    //     DistanceChangePlus(m_FailedStruggelModePercentPush);
    // }

    // private void OnSuccesLetterRule()
    // {
    //     if (GhostCombatSystem.Instance.ActiveStruggleMode) return;
    //     DistanceChangePlus(m_GhostTemplate.DistancePercentChangePlusPush);
    // }

    // private void OnFailedLetterRule()
    // {
    //     if (GhostCombatSystem.Instance.ActiveStruggleMode) return;
    //     DistanceChangePlus(m_GhostTemplate.DistancePercentChangePlusPull);
    // }

    // private void DistanceChangePlus(float amount)
    // {
    //     if (GhostCombatSystem.Instance.ActiveStruggleMode) return;
    //     m_DistancePercentChangePlus = amount;

    //     if (amount != 0f)
    //         OnGhostMovePlus?.Invoke(amount);
    // }

    // private void GhostReachedPlayer()
    // {
    //     m_GhostReachPlayer = true;
    //     Debug.Log("Ghost reached the player!");
    //     OnReachPlayer?.Invoke();
    // }

    // public void IncreaseGhostMoveTimeDueAbility(float addTime, float duration = 15f)
    // {
    //     m_GhostMoveChangeTime = duration;
    //     m_GhostMoveChangeCooldown = m_GhostMoveChangeTime;
    //     OnGhostSpeedIncrease?.Invoke(addTime);

    //     if (m_GhostMoveChangeDueAbility) return;
    //     m_GhostMoveChangeDueAbility = true;
    //     m_GhostMoveAllTime += addTime;
    //     if (m_GhostMoveAllTime < 0)
    //         m_GhostMoveAllTime = 0.01f;
    // }

    // private void ResetGhostMoveTime()
    // {
    //     m_GhostMoveAllTime = m_GhostTemplate.GhostMoveTime;
    //     m_GhostMoveChangeDueAbility = false;
    //     OnGhostSpeedIncrease?.Invoke(1);
    // }

    // /// <summary>
    // /// Immediately pushes the ghost forward by [amount], clamped and with events fired.
    // /// </summary>
    // public void AdvanceNow(float amount)
    // {
    //     // queue it into the “plus” buffer so it’s applied next Update
    //     DistanceChangePlus(amount);
    // }
}
