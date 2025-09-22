using UnityEngine;
using UnityEngine.AI;

//TODO() - magic make child - magicShoot, magicAOE, magicBuff
[CreateAssetMenu(fileName = "Magic", menuName = "EnemyAI/Magic")]
public class MagicSO : AttackSO
{
    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        if (enemy2D.AttackCooldown <= 0 && enemy2D.AttackAnimCooldown <= 0)
        {
            enemy2D.AttackAnimCooldown = AttackAnimTime;
            enemy2D.AttackCooldown = AttackTime;

            if (enemy2D.ObjectMagic != null)
            {
                if (m_CommandDataFlyweight is CommandPooledObjDataFlyweight commandPooledObjDataFlyweight)
                    enemy2D.ObjectAbility.DoAbility(commandPooledObjDataFlyweight, 50, commandPooledObjDataFlyweight.PooledObjNameDataFlyweight.Name);
            }
            else
            {
                Debug.LogWarning("ObjectMagic is null! Magic attack not executed.");
            }
        }
    }
}