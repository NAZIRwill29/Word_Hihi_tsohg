using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "StandBy", menuName = "EnemyAI/StandBy")]
public class StandBySO : IdleSO
{
    public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        if (m_stoppingDistance > 0.5f)
            navMeshAgent.stoppingDistance = m_stoppingDistance - 0.5f;
        if (navMeshAgent.stoppingDistance > 2f)
        {
            navMeshAgent.stoppingDistance -= 0.2f;
            enemy2D.GoalDist = navMeshAgent.stoppingDistance += 0.1f;
        }
        else if (navMeshAgent.stoppingDistance > 0)
            enemy2D.GoalDist = navMeshAgent.stoppingDistance + 0.1f;
    }
}
