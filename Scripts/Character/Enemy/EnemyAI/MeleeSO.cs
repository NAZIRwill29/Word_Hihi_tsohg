using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Melee", menuName = "EnemyAI/Melee")]
public class MeleeSO : AttackSO
{
    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);

        if (enemy2D.AttackCooldown <= 0f && enemy2D.AttackAnimCooldown <= 0f)
        {
            enemy2D.AttackAnimCooldown = AttackAnimTime;
            enemy2D.AttackCooldown = AttackTime;

            if (enemy2D.ObjectMelee != null)
            {
                enemy2D.ObjectAbility.DoAbility(m_CommandDataFlyweight);
            }
            else
            {
                Debug.LogWarning("ObjectMelee is null! Melee attack not executed.");
            }
        }
    }
}
