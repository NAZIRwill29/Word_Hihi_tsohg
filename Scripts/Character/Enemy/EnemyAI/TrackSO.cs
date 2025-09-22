using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Track", menuName = "EnemyAI/Track")]
public class TrackSO : EnemyAISO
{
    //public EnemyChaseType enemyChaseType;
    [SerializeField] protected float m_AddTrackLine = 10;
    public float TrackTime = 1;

    public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.SetTarget(enemy2DMovement, enemy2D, navMeshAgent);
        enemy2D.AddLineDetection = m_AddTrackLine;
        enemy2DMovement.Target = enemy2D.TempTarget;
    }

    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        //in state track player last pos sight
        enemy2D.TempTarget.transform.position = enemy2D.DetectionSense.LastPlayerPosSight;
        // Debug.Log("dist goal " + enemy2D.CenterTransf.transform.position +
        //     " - " + enemy2D.DetectionSense.LastPlayerPosSight +
        //     " = " + Vector2.Distance(enemy2D.CenterTransf.transform.position, enemy2D.DetectionSense.LastPlayerPosSight));   
    }
}