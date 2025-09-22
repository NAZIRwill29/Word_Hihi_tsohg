using UnityEngine;

[CreateAssetMenu(fileName = "Roam", menuName = "EnemyAI/Roam")]
public class RoamSO : PatrolSO
{
    public override void ChangeNextGoal(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D)
    {
        int curGoal = enemy2DMovement.NextGoal;
        enemy2DMovement.NextGoal = RandomGenerator.GenerateRandomNumber(0, enemy2DMovement.GetSetGoal().Goals.Length);
        //get next goal until not same as current goal
        while (enemy2DMovement.NextGoal == curGoal)
        {
            enemy2DMovement.NextGoal = RandomGenerator.GenerateRandomNumber(0, enemy2DMovement.GetSetGoal().Goals.Length);
        }
    }
}