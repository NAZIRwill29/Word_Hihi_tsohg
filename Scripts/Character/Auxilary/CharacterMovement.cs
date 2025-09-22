using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// need ObjectSpeed - Speed, CanWalk
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    protected Character m_Character;
    [SerializeField] protected ObjectSpeed m_ObjectSpeed;

    [SerializeField] protected Vector2 m_StartMoveDirection = new(1, 0);
    //right 10, down 0-1, left -10, up 01
    [SerializeField] private Vector2 m_MoveDirection = new Vector2(1, 0);
    public Vector2 MoveDirection
    {
        get => m_MoveDirection;
        set
        {
            if (value != Vector2.zero) // Prevent setting it to (0,0)
                m_MoveDirection = value.normalized; // Ensure it's always normalized
        }
    }
    [SerializeField] protected float m_MoveSpeed { get; set; }
    [ReadOnly] public float CurrentSpeed;
    public bool IsCanWalk { get; set; } = true;

    [Tooltip("Rate of change for move speed"), SerializeField]
    protected FloatDataFlyweight m_AccelerationFloatDataFlyweight;

    protected virtual void Awake()
    {
        m_Character = GetComponent<Character>();

        if (m_Character == null)
        {
            Debug.LogError("Character component not found on GameObject.");
            return;
        }

        if (m_Character.Animator == null)
            Debug.LogWarning("Animator component not found on Character.");

        StartMoveDirection();
    }

    protected virtual void OnEnable()
    {
        if (m_ObjectSpeed == null) return;

        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectSpeed, "Speed")
            ?.AddListener(SpeedChange);

        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectSpeed, "CanWalk")
            ?.AddListener(CanWalkChange);
    }

    protected virtual void OnDisable()
    {
        if (m_ObjectSpeed == null) return;

        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectSpeed, "Speed")
            ?.RemoveListener(SpeedChange);

        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectSpeed, "CanWalk")
            ?.RemoveListener(CanWalkChange);
    }

    protected virtual void Start()
    {
        if (m_ObjectSpeed == null)
        {
            Debug.LogWarning("ObjectSpeed component not found on Character.");
            return;
        }
        DataNumericalVariable numericalVariable = VariableFinder.GetVariableContainNameFromList(m_ObjectSpeed.StatsData.DataNumVars, "Speed");

        if (numericalVariable != null)
            m_MoveSpeed = numericalVariable.NumVariable;
        else
            Debug.LogWarning("Speed variable not found on ObjectSpeed.");
    }

    protected void SpeedChange(DataNumericalVariable variable)
    {
        if (variable == null) return;
        m_MoveSpeed = variable.NumVariable;
    }

    protected void CanWalkChange(DataBoolVariable variable)
    {
        if (variable == null || variable.IsCan == BoolNull.IsNull) return;
        IsCanWalk = variable.IsCan == BoolNull.IsTrue;
    }

    public void StartMoveDirection()
    {
        SetMoveDirection(m_StartMoveDirection.x, m_StartMoveDirection.y);
    }

    public void SetMoveDirection(float directionX, float directionY)
    {
        MoveDirection = new Vector2(directionX, directionY);
        //MoveDirection.Normalize();

        if (m_Character?.Animator != null)
        {
            m_Character.Animator.SetFloat("Look X", MoveDirection.x);
            m_Character.Animator.SetFloat("Look Y", MoveDirection.y);
        }
    }

    public virtual void SetMoveSpeed(Vector2 move)
    {
        if (m_Character == null || m_Character.Animator == null || m_Character.ObjectAudioMulti == null)
            return;

        bool wasMoving = CurrentSpeed > 0.1f; // Check if character was moving before
        CurrentSpeed = GameManager.Instance.IsPause ? 0 : move.magnitude;

        m_Character.Animator.SetFloat("Speed", CurrentSpeed);
        //Debug.Log("Speed " + MoveSpeed);

        // if (MoveSpeed > 0.1f)
        // {
        //     m_Character.m_CharacterAudioMulti.PlayRandomClip("walk");
        //     // if (!wasMoving)
        //     //     m_Character.ChangeState("Walk");
        // }
        // else if (MoveSpeed < 0.1f)
        //     m_Character.ChangeState("Idle");
    }
}
