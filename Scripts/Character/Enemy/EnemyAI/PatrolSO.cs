using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Patrol", menuName = "EnemyAI/Patrol")]
public class PatrolSO : EnemyAISO
{
    // public override void SetTarget(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    // {
    //     base.SetTarget(enemy2DMovement, enemy2D, navMeshAgent);
    // }

    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        float distance = Vector2.Distance(
            enemy2D.CenterTransf.transform.position,
            enemy2DMovement.GetSetGoal().Goals[enemy2DMovement.NextGoal].transform.position
        );
        // Debug.Log("dist goal " + enemy2D.CenterTransf.transform.position + " - "
        //     + enemy2DMovement.Goals[enemy2DMovement.NextGoal].transform.position + " = " + distance);
        if (distance < enemy2D.GoalDist)
        {
            ChangeNextGoal(enemy2DMovement, enemy2D);
        }
        enemy2DMovement.Target = enemy2DMovement.GetSetGoal().Goals[enemy2DMovement.NextGoal];
    }

    public override void ChangeNextGoal(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D)
    {
        enemy2DMovement.NextGoal = enemy2DMovement.NextGoal < enemy2DMovement.GetSetGoal().Goals.Length - 1 ?
            enemy2DMovement.NextGoal + 1 : 0;
        //Debug.Log("change goal");
    }
}