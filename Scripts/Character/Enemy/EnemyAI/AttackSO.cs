using UnityEngine;
using UnityEngine.AI;

//[CreateAssetMenu(fileName = "Attack", menuName = "EnemyAI/Attack")]
public class AttackSO : EnemyAISO
{
    [Tooltip("dist for tirgger attack"), Range(0.7f, 49f)] public float AttackLine = 2.75f;
    public float AttackTime = 2f;
    public float AttackChangeDurationTime = 7f;
    public float AttackAnimTime = 1f;
    [SerializeField] protected CommandDataFlyweight m_CommandDataFlyweight;

    public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        //Debug.Log("SetTarget");
        if (enemy2D.AttackLine > 0.5f)
            navMeshAgent.stoppingDistance = enemy2D.AttackLine - 0.5f;
        if (navMeshAgent.stoppingDistance > 2f)
        {
            navMeshAgent.stoppingDistance -= 0.2f;
            enemy2D.GoalDist = navMeshAgent.stoppingDistance += 0.1f;
        }
        else if (navMeshAgent.stoppingDistance > 0)
            enemy2D.GoalDist = navMeshAgent.stoppingDistance + 0.1f;
    }

    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        SetTarget(enemy2DMovement, enemy2D, navMeshAgent);
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
    }
}