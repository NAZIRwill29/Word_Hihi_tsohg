using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private ZoneTrigger m_ZoneTrigger;
    [SerializeField] private ObjectStatTrigger m_ObjectStatTrigger;
    [SerializeField] protected NameDataFlyweight m_AnimationNameDataTrigger, m_AnimationNameDataExit;

    void OnEnable()
    {
        if (m_ZoneTrigger)
        {
            m_ZoneTrigger.OnTriggerEffect += ProcessTrigger;
            m_ZoneTrigger.OnTriggerExit += ProcessExit;
        }
        if (m_ObjectStatTrigger)
        {
            m_ObjectStatTrigger.OnTriggerEffect += ProcessTrigger;
            m_ObjectStatTrigger.OnTriggerExit += ProcessExit;
        }
    }

    void OnDisable()
    {
        if (m_ZoneTrigger)
        {
            m_ZoneTrigger.OnTriggerEffect -= ProcessTrigger;
            m_ZoneTrigger.OnTriggerExit -= ProcessExit;
        }
        if (m_ObjectStatTrigger)
        {
            m_ObjectStatTrigger.OnTriggerEffect -= ProcessTrigger;
            m_ObjectStatTrigger.OnTriggerExit -= ProcessExit;
        }
    }

    public virtual void ProcessTrigger(Collider2D other)
    {
        IAnimatioanable animatioanable = other.GetComponent<IAnimatioanable>();
        if (animatioanable != null && m_AnimationNameDataTrigger)
        {
            animatioanable.InitAnimation(m_AnimationNameDataTrigger.Name);
        }
        else
            Debug.Log("not IAnimatioanable | no m_AnimationNameDataTrigger in " + other);
    }

    public virtual void ProcessExit(Collider2D other)
    {
        IAnimatioanable animatioanable = other.GetComponent<IAnimatioanable>();
        if (animatioanable != null && m_AnimationNameDataExit)
        {
            animatioanable.InitAnimation(m_AnimationNameDataExit.Name);
        }
    }
}
