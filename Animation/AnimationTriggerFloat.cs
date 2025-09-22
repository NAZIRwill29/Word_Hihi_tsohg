using UnityEngine;

public class AnimationTriggerFloat : AnimationTrigger
{
    [SerializeField] private float m_FloatTrigger, m_FloatExit;
    public override void ProcessTrigger(Collider2D other)
    {
        IAnimationableFloat animationableFloat = other.GetComponent<IAnimationableFloat>();
        if (animationableFloat != null && m_AnimationNameDataTrigger)
        {
            animationableFloat.InitAnimationFloat(m_AnimationNameDataTrigger.Name, m_FloatTrigger);
        }
    }

    public override void ProcessExit(Collider2D other)
    {
        IAnimationableFloat animationableFloat = other.GetComponent<IAnimationableFloat>();
        if (animationableFloat != null && m_AnimationNameDataExit)
        {
            animationableFloat.InitAnimationFloat(m_AnimationNameDataExit.Name, m_FloatExit);
        }
    }
}
