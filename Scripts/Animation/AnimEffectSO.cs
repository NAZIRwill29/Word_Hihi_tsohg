using UnityEngine;

[CreateAssetMenu(fileName = "AnimEffectSO", menuName = "AnimEffect/AnimEffectSO", order = 1)]
public class AnimEffectSO : ScriptableObject
{
    [SerializeField] private string m_AnimName;
    [SerializeField] private string m_AnimOffName;

    public void AnimEffect(Animator animator)
    {
        animator.SetTrigger(m_AnimName);
    }

    public void AnimEffectOff(Animator animator)
    {
        if (!string.IsNullOrEmpty(m_AnimOffName))
            animator.SetTrigger(m_AnimOffName);
    }
}
