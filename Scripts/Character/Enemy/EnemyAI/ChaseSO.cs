using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Chase", menuName = "EnemyAI/Chase")]
public class ChaseSO : EnemyAISO
{
    //public EnemyChaseType enemyChaseType;
    [SerializeField] protected float m_AddChaseLine = 5;

    public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.SetTarget(enemy2DMovement, enemy2D, navMeshAgent);
        //in state chase player
        enemy2D.AddLineDetection = m_AddChaseLine;
        enemy2DMovement.Target = enemy2D.TempTarget;
    }

    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        enemy2D.TempTarget.transform.position = GameManager.Instance.player2D.Center.transform.position;
    }
}