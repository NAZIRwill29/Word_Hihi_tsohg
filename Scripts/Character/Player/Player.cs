using UnityEngine;


[RequireComponent(typeof(PlayerInput), typeof(CharacterAudio), typeof(PlayerMovement))]

public class Player : MonoBehaviour
{
    [SerializeField]
    [Tooltip("LayerMask to identify obstacles in the game environment.")]
    LayerMask m_ObstacleLayer;

    // Components for handling different aspects of player functionality.
    PlayerInput m_PlayerInput;
    PlayerMovement m_PlayerMovement;
    CharacterAudio m_PlayerAudio;
    CharacterFX m_PlayerFX;

    private void Awake()
    {
        Initialize();
    }

    // Sets up component references.
    private void Initialize()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerAudio = GetComponent<CharacterAudio>();
        m_PlayerFX = GetComponent<CharacterFX>();
    }

    // This method is called when the controller collides with another collider.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object is in the obstacle layer.
        if (m_ObstacleLayer.ContainsLayer(hit.gameObject))
        {
            // Play a random audio clip on collision.
            m_PlayerAudio.PlayRandomClip();

            // Trigger visual effect, if defined.
            if (m_PlayerFX != null)
                m_PlayerFX.PlayEffect();

        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        // Get the input vector from the PlayerInput component.
        Vector3 inputVector = m_PlayerInput.InputVector;

        // Move the player based on the input vector.
        m_PlayerMovement.Move(inputVector);
    }
}
