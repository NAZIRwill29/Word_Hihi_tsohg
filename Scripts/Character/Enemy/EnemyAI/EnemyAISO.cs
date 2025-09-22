using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAISO : ScriptableObject
{
    [SerializeField, Range(0.6f, 30f)] protected float m_stoppingDistance = 0.6f;
    [SerializeField][Range(0.1f, 1.5f)] protected float m_changeGoalTime = 0.5f;
    [SerializeField, Range(0.1f, 1.5f)] protected float m_staticTime = 0.5f;

    public virtual void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        //Debug.Log("SetTarget");
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

    public virtual void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        enemy2D.PlayerPrevPos = enemy2D.transform.position;
        //Debug.Log("Move");
    }

    public virtual void CheckStuck(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D)
    {
        if (enemy2D.transform.position == enemy2D.PlayerPrevPos)
        {
            //Debug.Log("static");
            enemy2D.StaticCd -= Time.deltaTime;
        }
        else
            enemy2D.StaticCd = m_staticTime;

        if (enemy2D.StaticCd < 0)
        {
            //Debug.Log("change due to stuck");
            enemy2D.StaticCd = m_staticTime;

            enemy2D.ChangeGoalCd -= Time.deltaTime;
            if (enemy2D.ChangeGoalCd >= 0)
                return;
            enemy2D.ChangeGoalCd = m_changeGoalTime;

            ChangeNextGoal(enemy2DMovement, enemy2D);
        }
    }

    public virtual void ChangeNextGoal(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D)
    {
        //Debug.Log("ChangeNextGoal");
    }
}
