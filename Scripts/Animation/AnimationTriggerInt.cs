using UnityEngine;

public class AnimationTriggerInt : AnimationTrigger
{
    [SerializeField] private int m_IntTrigger, m_IntExit;
    public override void ProcessTrigger(Collider2D other)
    {
        IAnimationableInt animationableInt = other.GetComponent<IAnimationableInt>();
        if (animationableInt != null && m_AnimationNameDataTrigger)
        {
            animationableInt.InitAnimationInt(m_AnimationNameDataTrigger.Name, m_IntTrigger);
        }
    }

    public override void ProcessExit(Collider2D other)
    {
        IAnimationableInt animationableInt = other.GetComponent<IAnimationableInt>();
        if (animationableInt != null && m_AnimationNameDataExit)
        {
            animationableInt.InitAnimationInt(m_AnimationNameDataExit.Name, m_IntExit);
        }
    }
}