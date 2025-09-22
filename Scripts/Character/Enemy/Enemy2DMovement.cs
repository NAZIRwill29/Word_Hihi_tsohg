using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SetGoal
{
    public GameObject[] Goals;
}

//NOTE() - patrol or roam set speed <= 40
public class Enemy2DMovement : CharacterMovement
{
    private NavMeshAgent m_NavMeshAgent;
    [HideInInspector] public GameObject Target;
    public SetGoal[] SetGoals;
    public GameObject IdleGoal;
    public int NextGoal = 1;
    protected int m_CurrentSetGoal = 0;

    protected override void Start()
    {
        base.Start();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.updateRotation = false;
        m_NavMeshAgent.updateUpAxis = false;
        m_NavMeshAgent.acceleration = m_AccelerationFloatDataFlyweight.Float;
    }

    // void Update()
    // {
    //     if (!m_Character.IsAlive) return;
    //     if (!isCanMove) return;
    //     EnemyAISO.SetTarget(this, m_Character, m_NavMeshAgent);
    // }

    void LateUpdate()
    {
        if (GameManager.Instance.IsPause)
        {
            m_NavMeshAgent.speed = 0;
            return;
        }
        if (!m_Character.IsActive) return;
        if (!m_Character.IsAlive) return;
        if (m_Character.IsStunned) return;
        if (!IsCanWalk) return;
        if (m_Character is Enemy2D enemy2D && enemy2D.IsIdle)
        {
            //stop moving
            //m_NavMeshAgent.SetDestination(enemy2D.transform.position);
            m_NavMeshAgent.enabled = false;
            return;
        }
        m_NavMeshAgent.enabled = true;
        MoveToTarget(m_Character.CenterTransf.transform.position);
    }

    // void FixedUpdate()
    // {
    //     if (!m_Character.IsAlive) return;
    //     if (!isCanMove) return;
    //     EnemyAISO.CheckStuck(this, m_Character);
    // }

    public void MoveToTarget(Vector2 owner)
    {
        //Debug.Log("MoveToTarget");
        Vector2 targetPos = Target.transform.position;
        m_NavMeshAgent.SetDestination(targetPos);
        SetMoveDirection(owner, targetPos);
        //m_Character.m_CharacterAudioMulti.PlayRandomClip("walk");
    }

    private void SetMoveDirection(Vector2 ownerPos, Vector2 goalPos)
    {
        float distX = ownerPos.x - goalPos.x;
        float uDistX = distX < 0 ? -distX : distX;
        float distY = ownerPos.y - goalPos.y;
        float uDistY = distY < 0 ? -distY : distY;

        if (uDistX > uDistY)
            SetMoveDirection(distX < 0 ? 1 : -1, 0);
        else
            SetMoveDirection(0, distY < 0 ? 1 : -1);

        SetMoveSpeed(new Vector2(distX, distY));
    }

    public override void SetMoveSpeed(Vector2 move)
    {
        if (m_Character == null || m_Character.Animator == null || m_Character.ObjectAudioMulti == null)
            return;

        bool wasMoving = CurrentSpeed > 0.1f; // Check if character was moving before
        CurrentSpeed = move.magnitude > 0.1f ? m_MoveSpeed : 0;
        m_NavMeshAgent.speed = CurrentSpeed / 10;

        m_Character.Animator.SetFloat("Speed", CurrentSpeed);
        //Debug.Log("Speed " + MoveSpeed);
    }

    public SetGoal GetSetGoal()
    {
        return SetGoals[m_CurrentSetGoal];
    }

    public void ChangeCurrentSetGoal(int num)
    {
        if (num >= 0 && num < SetGoals.Length)
            m_CurrentSetGoal = num;
    }
}
