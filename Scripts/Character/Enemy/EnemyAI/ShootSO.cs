using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Shoot", menuName = "EnemyAI/Shoot")]
public class ShootSO : AttackSO
{
    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        if (enemy2D.AttackCooldown <= 0 && enemy2D.AttackAnimCooldown <= 0)
        {
            enemy2D.AttackAnimCooldown = AttackAnimTime;
            enemy2D.AttackCooldown = AttackTime;

            if (enemy2D.ObjectShoot != null)
            {
                if (m_CommandDataFlyweight is CommandPooledObjDataFlyweight commandPooledObjDataFlyweight)
                    enemy2D.ObjectAbility.DoAbility(commandPooledObjDataFlyweight, 50, commandPooledObjDataFlyweight.PooledObjNameDataFlyweight.Name);
            }
            else
            {
                Debug.LogWarning("ObjectShoot is null! Shoot attack not executed.");
            }
        }
    }
}