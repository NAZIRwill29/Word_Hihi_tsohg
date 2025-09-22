using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyAISOData
{
    [SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : null;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }
    public EnemyAISO EnemyAISO;
}
[System.Serializable]
public class AttackSOData
{
    [SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : null;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }
    public AttackSO AttackSO;
}

[CreateAssetMenu(fileName = "Enemy2DAI", menuName = "Enemy2DAI/Enemy2DAI")]
public class Enemy2DAI : ScriptableObject
{
    [SerializeField] private EnemyAISOData[] EnemyAISOs;
    [SerializeField] private AttackSOData[] AttackSOs;
    private Dictionary<string, EnemyAISO> m_EnemyAISODict = new();
    private Dictionary<string, AttackSO> m_AttackSODict = new();

    public void Initialize()
    {
        m_EnemyAISODict.Clear();
        foreach (var item in EnemyAISOs)
        {
            if (item != null && item.EnemyAISO != null)
            {
                m_EnemyAISODict[item.Name] = item.EnemyAISO;
            }
        }
        m_AttackSODict.Clear();
        foreach (var item in AttackSOs)
        {
            if (item != null && item.AttackSO != null)
            {
                m_AttackSODict[item.Name] = item.AttackSO;
            }
        }
    }

    public void Execute(Enemy2D enemy2D, Enemy2DMovement enemy2DMovement)
    {
        if (enemy2D == null || enemy2DMovement == null) return;

        Player2D player2D = GameManager.Instance.player2D;

        if (enemy2D.ObjectSpeed.IsStandBy)
        {
            SetEnemyState(enemy2D, enemy2DMovement, "StandBy");
            return;
        }

        if (enemy2D.IsDetectPlayer)
        {
            if (enemy2D.IsFlee)
            {
                // Flee behavior
                enemy2D.IsChase = false;
                SetEnemyState(enemy2D, enemy2DMovement, "Flee");
                //Debug.Log("flee after detect player");
            }
            else
            {
                //RandomChangeAttackSO(enemy2D);

                float addAttackLine = 0;
                if (enemy2DMovement.MoveDirection == Vector2.up)
                    addAttackLine = enemy2D.OffsetTop + player2D.OffsetBottom;
                else if (enemy2DMovement.MoveDirection == Vector2.down)
                    addAttackLine = -enemy2D.OffsetBottom / 2 + player2D.OffsetTop;
                else if (enemy2DMovement.MoveDirection == Vector2.right)
                    addAttackLine = enemy2D.OffsetRight + player2D.OffsetRight;
                else if (enemy2DMovement.MoveDirection == Vector2.left)
                    addAttackLine = enemy2D.OffsetLeft + player2D.OffsetLeft;

                //if (TryGetEnemyAISO("Attack", out AttackSO attackSO))
                enemy2D.AttackLine = enemy2D.AttackSO.AttackLine + addAttackLine;

                float distanceFromPlayer = Vector2.Distance(enemy2D.Center.transform.position, player2D.Center.transform.position);
                if (distanceFromPlayer <= enemy2D.AttackLine)
                {
                    // Attack behavior
                    enemy2D.AttackCooldown -= Time.deltaTime;
                    enemy2D.AttackAnimCooldown -= Time.deltaTime;

                    // Debug.Log("set Attack when dist " + distanceFromPlayer);
                    //Debug.Log("Attack after detect player");
                    SetAttackEnemyState(enemy2D, enemy2DMovement);
                }
                else if (enemy2D.IsTrack)
                {
                    // Track behavior
                    enemy2D.TrackCooldown -= Time.deltaTime;
                    //Debug.Log("Track after detect player");

                    if (enemy2D.TrackCooldown < 0)
                    {
                        enemy2D.IsTrack = false;
                        enemy2D.IsAfterTrack = true;
                    }

                    SetEnemyState(enemy2D, enemy2DMovement, "Track");
                }
                else
                {
                    // Chase behavior
                    enemy2D.IsChase = true;
                    enemy2D.IsTrack = true;
                    enemy2D.IsIdle = false;
                    enemy2D.IsPlayerEscape = false;
                    SetEnemyState(enemy2D, enemy2DMovement, "Chase");
                    //Debug.Log("Chase after detect player");

                    if (TryGetEnemyAISO("Track", out TrackSO trackSO))
                    {
                        enemy2D.TrackCooldown = trackSO.TrackTime;
                    }
                }
            }
        }
        else
        {
            enemy2D.IsChase = false;

            if (enemy2D.IsTrack)
            {
                // Player escaped while tracking
                //enemy2D.IsPlayerEscape = true;
                enemy2D.IsIdle = false;
                enemy2D.TrackCooldown -= Time.deltaTime;

                //Debug.Log("Track undetected player");

                if (enemy2D.TrackCooldown < 0)
                {
                    enemy2D.IsTrack = false;
                    enemy2D.IsAfterTrack = true;
                }

                SetEnemyState(enemy2D, enemy2DMovement, "Track");
            }
            else
            {
                // Patrol or Idle
                enemy2D.AddLineDetection = 0;
                enemy2D.IsPlayerEscape = true;
                enemy2D.PlayerEscapeAnimation();

                if (enemy2D.IsAfterTrack)
                {
                    //change to nearest patrol pt
                    ChangeToNearestGoal(enemy2D, enemy2DMovement);
                    enemy2D.IsAfterTrack = false;
                }

                if (enemy2D.IsStartIdle)
                {
                    //Debug.Log("idle");
                    SetEnemyState(enemy2D, enemy2DMovement, "Idle");
                }
                else
                {
                    //Debug.Log("patrol");
                    SetEnemyState(enemy2D, enemy2DMovement, "Patrol");
                }
            }
        }

        enemy2D.ActiveEnemyAISO?.MoveUnit(enemy2DMovement, enemy2D, enemy2D.NavMeshAgent);
    }

    public void FixedCheck(Enemy2D enemy2D, Enemy2DMovement enemy2DMovement)
    {
        if (enemy2DMovement == null || !enemy2DMovement.IsCanWalk || enemy2D.IsIdle) return;
        enemy2D.ActiveEnemyAISO?.CheckStuck(enemy2DMovement, enemy2D);
    }

    public void NotifyHealthChanged(Enemy2D enemy2D, HealthData healthData)
    {
        if (enemy2D == null || healthData == null) return;

        Debug.Log("NotifyHealthChanged in Enemy2D");

        DataNumericalVariable variable = VariableFinder.GetVariableContainNameFromList(healthData.DataNumVars, "Health");

        if (variable == null) return;

        float percentage = (variable.NumVariable / variable.NumVariableMax) * 100;

        if (TryGetEnemyAISO("Flee", out FleeSO fleeSO))
        {
            if (percentage < fleeSO.HealthPercentageToFlee)
            {
                if (healthData.EffectorName != "Player") return;

                enemy2D.IsFlee = true;
                Debug.Log("Too low health - need to flee");
            }
            else
            {
                enemy2D.IsFlee = false;

                if (healthData.EffectorName != "Player") return;

                if (TryGetEnemyAISO("Track", out TrackSO trackSO))
                {
                    enemy2D.IsTrack = true;
                    enemy2D.TrackCooldown = trackSO.TrackTime;
                    Debug.Log("Health reduced - need to track");
                }
            }
        }
        // else
        // {
        //     ChangeAttackSO(enemy2D, "Berserk");
        // }
    }

    public void SetEnemyState(Enemy2D enemy2D, Enemy2DMovement enemy2DMovement, string stateName)
    {
        if (m_EnemyAISODict.TryGetValue(stateName, out EnemyAISO enemyAISO))
        {
            if (enemy2D.ActiveEnemyAISO == enemyAISO) return;

            enemy2D.ActiveEnemyAISO = enemyAISO;
            enemy2D.ActiveEnemyAISO.SetTarget(enemy2DMovement, enemy2D, enemy2D.NavMeshAgent);
        }
    }

    private void SetAttackEnemyState(Enemy2D enemy2D, Enemy2DMovement enemy2DMovement)
    {
        if (enemy2D.ActiveEnemyAISO == enemy2D.AttackSO) return;

        enemy2D.ActiveEnemyAISO = enemy2D.AttackSO;
        enemy2D.ActiveEnemyAISO.SetTarget(enemy2DMovement, enemy2D, enemy2D.NavMeshAgent);
    }

    //TODO()//
    private void RandomChangeAttackSO(Enemy2D enemy2D)
    {
        enemy2D.AttackChangeDurationCooldown -= Time.deltaTime;
        if (enemy2D.AttackChangeDurationCooldown <= 0)
        {
            Debug.Log("RandomChangeAttackSO");
            int index = RandomGenerator.GenerateRandomNumber(0, m_AttackSODict.Count);
            enemy2D.AttackSO = AttackSOs[index].AttackSO;
            enemy2D.AttackChangeDurationCooldown = enemy2D.AttackSO.AttackChangeDurationTime;
        }
    }

    //TODO()//
    private void ChangeAttackSO(Enemy2D enemy2D, string attackName)
    {
        if (m_AttackSODict.TryGetValue(attackName, out AttackSO attackSO))
            enemy2D.AttackSO = attackSO;
    }

    private bool TryGetEnemyAISO<T>(string stateName, out T enemyAISO) where T : EnemyAISO
    {
        if (m_EnemyAISODict.TryGetValue(stateName, out EnemyAISO baseAISO) && baseAISO is T specificAISO)
        {
            enemyAISO = specificAISO;
            return true;
        }

        enemyAISO = null;
        return false;
    }

    public void ChangeToNearestGoal(Enemy2D enemy2D, Enemy2DMovement enemy2DMovement)
    {
        //GameObject nearestGoal = null;
        int nearestGoalNum = 0;
        float shortestDistance = Mathf.Infinity;

        Vector2 enemyPosition = enemy2D.Center.transform.position;

        for (int i = 0; i < enemy2DMovement.GetSetGoal().Goals.Length; i++)
        {
            if (enemy2DMovement.GetSetGoal().Goals[i] == null) continue; // Safety check

            float distance = Vector2.Distance(enemyPosition, enemy2DMovement.GetSetGoal().Goals[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                //nearestGoal = enemy2DMovement.Goals[i];
                nearestGoalNum = i;
            }
        }
        enemy2DMovement.NextGoal = nearestGoalNum;
        //return nearestGoal;
    }
}
