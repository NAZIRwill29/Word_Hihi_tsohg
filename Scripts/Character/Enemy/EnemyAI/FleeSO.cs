using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Flee", menuName = "EnemyAI/Flee")]
public class FleeSO : EnemyAISO
{
    [SerializeField] private float m_FleeLine = 5;
    public float HealthPercentageToFlee;
    public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.SetTarget(enemy2DMovement, enemy2D, navMeshAgent);
        enemy2D.AddLineDetection = m_FleeLine;
        enemy2DMovement.Target = enemy2D.TempTarget;
    }

    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        if (enemy2D.IsDetectPlayer)
            enemy2D.TempTarget.transform.position = -enemy2D.DetectionSense.DirectionToPlayer;
    }
}
