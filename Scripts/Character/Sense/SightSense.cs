using UnityEngine;

public class SightSense : MonoBehaviour
{
    [SerializeField] private DetectionSense m_DetectionSense;
    [SerializeField] private float m_ViewAngle = 50f;
    public float LineOfSight = 1;
    [SerializeField] private CharacterMovement characterMovement;
    // Declare a static LayerMask variable
    private LayerMask obstaclePlayerMask;

    private void Awake()
    {
        // Initialize the LayerMask once in Awake or Start
        obstaclePlayerMask = LayerMask.GetMask("Obstacle", "PlayerTrigger");
    }

    public bool SeePlayerWideView(Vector2 centerPos, Vector2 playerPos, float addLineOfSight = 0)
    {
        if (m_DetectionSense == null)
        {
            Debug.LogWarning("DetectionSense is not assigned in HearSense.");
            return false;
        }

        if (GameManager.Instance.player2D.ObjectVisibility)
        {
            DataBoolVariable data = VariableFinder.GetVariableContainNameFromList(GameManager.Instance.player2D.ObjectVisibility.StatsData.DataBoolVars, "CanBeSeen");
            if (data != null)
            {
                if (!VariableChanger.IsBoolNull(data.IsCan)) return false;
            }
            else
            {
                Debug.LogWarning("IsCanBeSeen is not assigned in player2D.");
            }
        }
        else
        {
            Debug.LogWarning("player2D is not assigned in GameManager.");
        }

        m_DetectionSense.DistanceFromPlayer = Vector2.Distance(centerPos, playerPos);

        if (m_DetectionSense.DistanceFromPlayer < LineOfSight + addLineOfSight)
        {
            Vector2 directionToPlayer = (playerPos - centerPos).normalized;
            m_DetectionSense.DirectionToPlayer = directionToPlayer;

            Vector2 moveDir = characterMovement.MoveDirection;
            if (moveDir == Vector2.zero) moveDir = Vector2.down; // Default to facing downward if stationary

            float angleBetweenGuardAndPlayer = Vector2.Angle(moveDir, directionToPlayer);

            if (angleBetweenGuardAndPlayer < m_ViewAngle / 2)
            {
                Debug.DrawLine(transform.position, playerPos, Color.red, 0.1f);
                Debug.DrawRay(transform.position, directionToPlayer * (LineOfSight + addLineOfSight), Color.blue, 0.1f);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, LineOfSight + addLineOfSight, obstaclePlayerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Obstacle"))
                        return false; // Obstacle blocks vision
                    if (hit.collider.CompareTag("PlayerTrigger"))
                        return true; // Player detected
                }
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Draw the line of sight radius
        Gizmos.DrawWireSphere(transform.position, LineOfSight);

        // Calculate the left and right boundary directions of the view cone
        Vector2 forward = Vector2.down; // Default facing downward
        float halfAngle = m_ViewAngle / 2f;

        // Calculate left and right direction based on angle
        Vector2 leftDirection = Quaternion.Euler(0, 0, -halfAngle) * forward;
        Vector2 rightDirection = Quaternion.Euler(0, 0, halfAngle) * forward;

        // Calculate positions where the cone ends
        Vector3 leftEndpoint = transform.position + (Vector3)(leftDirection * LineOfSight);
        Vector3 rightEndpoint = transform.position + (Vector3)(rightDirection * LineOfSight);

        // Draw the field of view triangle
        Gizmos.DrawLine(transform.position, leftEndpoint);
        Gizmos.DrawLine(transform.position, rightEndpoint);
        Gizmos.DrawLine(leftEndpoint, rightEndpoint);
    }
}
