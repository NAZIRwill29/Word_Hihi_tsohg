using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Idle", menuName = "EnemyAI/Idle")]
public class IdleSO : EnemyAISO
{
    public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.SetTarget(enemy2DMovement, enemy2D, navMeshAgent);
        if (enemy2DMovement.IdleGoal)
            enemy2DMovement.Target = enemy2DMovement.IdleGoal;
        // else
        // {
        //     SetIdle(enemy2DMovement, enemy2D);
        // }
    }

    public override void ChangeNextGoal(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D)
    {
        //Debug.Log("stuck - go to idle");
        enemy2D.IsIdle = true;
        enemy2DMovement.StartMoveDirection();
        enemy2DMovement.SetMoveSpeed(Vector2.zero);
    }
}