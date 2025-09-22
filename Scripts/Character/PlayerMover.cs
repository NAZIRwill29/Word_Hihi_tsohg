using UnityEngine;

// moves the player one space and checks for walls
public class PlayerMover : MonoBehaviour
{
    [SerializeField] protected LayerMask m_ObstacleLayer;

    protected const float k_BoardSpacing = 1f;

    // Path Visualization
    protected PlayerPath m_PlayerPath;
    public PlayerPath PlayerPath => m_PlayerPath;


    protected void Start()
    {
        m_PlayerPath = gameObject.GetComponent<PlayerPath>();
    }

    // Simple movement along XZ-plane
    public void Move(Vector3 movement)
    {
        Vector3 destination = transform.position + movement;
        transform.position = destination;
    }

    public virtual bool IsValidMove(Vector3 movement)
    {
        return !Physics.Raycast(transform.position, movement, k_BoardSpacing, m_ObstacleLayer);
    }
}