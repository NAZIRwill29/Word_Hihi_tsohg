using UnityEngine;

public class AnimationTriggerBool : AnimationTrigger
{
    [SerializeField] private bool m_IsTrueTrigger, m_IsTrueExit;
    public override void ProcessTrigger(Collider2D other)
    {
        IAnimationableBool animationableBool = other.GetComponent<IAnimationableBool>();
        if (animationableBool != null && m_AnimationNameDataTrigger)
        {
            animationableBool.InitAnimationBool(m_AnimationNameDataTrigger.Name, m_IsTrueTrigger);
        }
    }

    public override void ProcessExit(Collider2D other)
    {
        IAnimationableBool animationableBool = other.GetComponent<IAnimationableBool>();
        if (animationableBool != null && m_AnimationNameDataExit)
        {
            animationableBool.InitAnimationBool(m_AnimationNameDataExit.Name, m_IsTrueExit);
        }
    }
}
