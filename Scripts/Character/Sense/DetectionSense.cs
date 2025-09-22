using UnityEngine;

public class DetectionSense : MonoBehaviour
{
    [SerializeField] private ObjectT m_ObjectT;
    [HideInInspector] public SightSense sightSense;
    [HideInInspector] public HearSense hearSense;
    public Vector3 LastPlayerPosSight { get; set; }
    public float DistanceFromPlayer { get; set; }
    public Vector2 DirectionToPlayer { get; set; }

    void Awake()
    {
        sightSense = GetComponent<SightSense>();
        hearSense = GetComponent<HearSense>();
    }

    public bool DetectPlayer(float addLineDetection = 0)
    {
        Vector2 playerPos = GameManager.Instance.player2D.Center.transform.position;
        Vector2 objPos = m_ObjectT.Center.transform.position;
        if (hearSense)
        {
            if (hearSense.HearPlayer(objPos, playerPos, addLineDetection))
            {
                LastPlayerPosSight = playerPos;
                //Debug.Log("hear");
                return true;
            }
        }
        if (sightSense)
        {
            if (sightSense.SeePlayerWideView(objPos, playerPos, addLineDetection))
            {
                LastPlayerPosSight = playerPos;
                //Debug.Log("see");
                return true;
            }
        }
        return false;
    }

    // bool DistDecision(float distFromPlayer, float distTigger, OperatorType oprt)
    // {
    //     switch (oprt)
    //     {
    //         case OperatorType.less:
    //             return distFromPlayer < distTigger;
    //         case OperatorType.lessEqual:
    //             return distFromPlayer <= distTigger;
    //         case OperatorType.more:
    //             return distFromPlayer > distTigger;
    //         case OperatorType.moreEqual:
    //             return distFromPlayer >= distTigger;
    //         case OperatorType.notEqual:
    //             return distFromPlayer != distTigger;
    //         //equal
    //         default:
    //             return distFromPlayer == distTigger;
    //     }
    // }
}
