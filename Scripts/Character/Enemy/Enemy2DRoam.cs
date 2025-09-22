using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//UNUSED
public class Enemy2DRoam : MonoBehaviour
{
    Character m_Character;
    NavMeshAgent m_agent;
    Enemy2DMovement enemy2DMovement;
    public EnemyRoamPattern enemyRoamPattern;
    public List<GameObject> goals = new List<GameObject>();
    private int m_NextGoal = 1;
    [SerializeField][Range(0.6f, 30f)] float m_stoppingDistance = 0.6f;
    [SerializeField][Range(0.5f, 3f)] float m_changeGoalTime = 0.5f;
    [SerializeField][Range(0.1f, 1.5f)] float m_checkTime = 0.5f;
    [SerializeField][Range(1f, 3f)] float m_staticTime = 3;

    float m_goalDist, m_changeGoalCd, m_checkCd, m_staticCd;
    Vector3 m_prevPos;

    void Start()
    {
        enemy2DMovement = GetComponent<Enemy2DMovement>();
        m_Character = GetComponent<Character>();
        m_agent = GetComponent<NavMeshAgent>();
        if (m_stoppingDistance > 0.5f)
            m_agent.stoppingDistance = m_stoppingDistance - 0.5f;
        if (m_agent.stoppingDistance > 2f)
        {
            m_agent.stoppingDistance = 2f;
            m_goalDist = 2.1f;
        }
        else if (m_agent.stoppingDistance > 0)
            m_goalDist = m_agent.stoppingDistance + 0.1f;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        if (enemy2DMovement.IsCanWalk)
            Roaming();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        if (enemy2DMovement.IsCanWalk)
            CheckStuck();
    }

    void Roaming()
    {
        float distance = Vector2.Distance(
            m_Character.CenterTransf.transform.position,
            goals[m_NextGoal].transform.position
        );
        // Debug.Log("dist goal " + enemy2D.enemyCenterTransf.transform.position + " - "
        //     + goals[m_NextGoal].transform.position + " = " + distance);
        if (distance < m_goalDist)
        {
            ChangeNextGoal();
        }
        enemy2DMovement.Target = goals[m_NextGoal];
        m_prevPos = transform.position;
    }

    void CheckStuck()
    {
        m_checkCd -= Time.deltaTime;
        if (m_checkCd >= 0)
            return;
        m_checkCd = m_checkTime;
        if (transform.position == m_prevPos)
        {
            //GenDebug("static");
            m_staticCd -= Time.deltaTime;
        }
        else
            m_staticCd = m_staticTime;
        if (m_staticCd < 0)
        {
            //Debug.Log("change due to stuck");
            m_staticCd = m_staticTime;
            ChangeNextGoal();
        }
    }

    void ChangeNextGoal()
    {
        m_changeGoalCd -= Time.deltaTime;
        if (m_changeGoalCd >= 0)
            return;
        m_changeGoalCd = m_changeGoalTime;
        //Debug.Log("change goal");
        switch (enemyRoamPattern)
        {
            case EnemyRoamPattern.random:
                int curGoal = m_NextGoal;
                m_NextGoal = RandomGenerator.GenerateRandomNumber(0, goals.Count);
                //get next goal until not same as current goal
                while (m_NextGoal == curGoal)
                {
                    m_NextGoal = RandomGenerator.GenerateRandomNumber(0, goals.Count);
                }
                break;
            //fix
            default:
                m_NextGoal = m_NextGoal < goals.Count - 1 ? m_NextGoal + 1 : 0;
                break;
        }
    }
}
