using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Ability", menuName = "EnemyAI/Ability")]
public class AbilitySO : AttackSO
{
    public override void MoveUnit(Enemy2DMovement enemy2DMovement, Enemy2D enemy2D, NavMeshAgent navMeshAgent)
    {
        base.MoveUnit(enemy2DMovement, enemy2D, navMeshAgent);
        if (enemy2D.AttackCooldown <= 0 && enemy2D.AttackAnimCooldown <= 0)
        {
            enemy2D.AttackAnimCooldown = AttackAnimTime;
            enemy2D.AttackCooldown = AttackTime;

            if (enemy2D.ObjectAbility != null)
            {
                // string attackName = string.IsNullOrEmpty(m_AttackName) ? "normal" : m_AttackName;
                // string idName = string.IsNullOrEmpty(m_IdName) ? "ability" : m_IdName;
                enemy2D.ObjectAbility.DoAbility(m_CommandDataFlyweight);
            }
            else
            {
                Debug.LogWarning("ObjectAbility is null! Ability attack not executed.");
            }
        }
    }
}