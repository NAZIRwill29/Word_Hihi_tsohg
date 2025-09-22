using UnityEngine;

public class PlayerMovement2D : CharacterMovement
{
    public float MultiSpeedFactor = 2.5f;
    public float m_ContinuousAccelerationMax = 0.05f;
    // [Header("Movement")]
    // [Tooltip("Horizontal speed")]
    // [SerializeField]
    // private float m_MoveSpeed = 5f;

    // [Tooltip("Deceleration rate when no input is provided")]
    // [SerializeField]
    // private float m_Deceleration = 0.5f;
    private float m_ContinuousAcceleration;

    //private float m_CurrentSpeed = 0f;
    private Rigidbody2D m_Rigidbody2d;

    public Rigidbody2D Rigidbody2d => m_Rigidbody2d;

    // [Tooltip("Rate of change for move speed"), SerializeField]
    // protected FloatDataFlyweight m_StartAccelerationFloatDataFlyweight;

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 inputVector, Vector2 move)
    {
        if (IsCanWalk)
        {
            if (inputVector == Vector2.zero)
            {
                // Apply deceleration when there is no input
                // if (CurrentSpeed > 0)
                // {
                //     CurrentSpeed -= m_Deceleration * Time.deltaTime;
                //     CurrentSpeed = Mathf.Max(CurrentSpeed, 0); // Ensure speed doesn't go negative
                // }
                CurrentSpeed = 0;
            }
            else
            {
                //CurrentSpeed = move.magnitude;
                m_ContinuousAcceleration = Time.deltaTime * m_AccelerationFloatDataFlyweight.Float;
                // float acceleration = CurrentSpeed < m_StartAccelerationFloatDataFlyweight.Float
                //     ? Time.deltaTime * m_StartAccelerationFloatDataFlyweight.Float
                //     : m_ContinuousAcceleration;
                if (m_ContinuousAcceleration > m_ContinuousAccelerationMax)
                    m_ContinuousAcceleration = m_ContinuousAccelerationMax;
                // Smoothly transition to the target speed when there is input
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, m_MoveSpeed * MultiSpeedFactor, m_ContinuousAcceleration);
            }

            Vector2 position = (Vector2)m_Rigidbody2d.position + CurrentSpeed * Time.deltaTime * move;
            //make move the rigidbody to avoid jittering
            m_Rigidbody2d.MovePosition(position);

            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                SetMoveDirection(move.x, move.y);
            }
            // m_Character.m_CharacterAudioMulti.PlayRandomClip("walk");
        }

        SetMoveSpeed(move);
    }

    public override void SetMoveSpeed(Vector2 move)
    {
        if (m_Character == null || m_Character.Animator == null || m_Character.ObjectAudioMulti == null)
            return;

        bool wasMoving = CurrentSpeed > 0.1f; // Check if character was moving before
        //CurrentSpeed = move.magnitude;

        m_Character.Animator.SetFloat("Speed", CurrentSpeed);
    }
}

