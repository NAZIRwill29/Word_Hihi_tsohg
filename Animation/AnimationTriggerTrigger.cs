using UnityEngine;

public class AnimationTriggerTrigger : AnimationTrigger
{
    public override void ProcessTrigger(Collider2D other)
    {
        IAnimationableTrigger animationableTrigger = other.GetComponent<IAnimationableTrigger>();
        if (animationableTrigger != null && m_AnimationNameDataTrigger)
        {
            animationableTrigger.InitAnimationTrigger(m_AnimationNameDataTrigger.Name);
        }
    }

    public override void ProcessExit(Collider2D other)
    {
        IAnimationableTrigger animationableTrigger = other.GetComponent<IAnimationableTrigger>();
        if (animationableTrigger != null && m_AnimationNameDataExit)
        {
            animationableTrigger.InitAnimationTrigger(m_AnimationNameDataExit.Name);
        }
    }
}
