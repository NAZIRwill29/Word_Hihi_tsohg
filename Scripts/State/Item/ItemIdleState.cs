using UnityEngine;

[CreateAssetMenu(fileName = "ItemIdleState", menuName = "State/ItemStates/ItemIdleState")]
public class ItemIdleState : ScriptableState
{
    [SerializeField] private NameDataFlyweight m_SoundNameDataFlyweight;
    public override void Enter(ObjectT objectT)
    {
        objectT.Animator.SetBool("Move", false);
        Debug.Log("Enter Idle");
    }

    // per-frame logic, include condition to transition to a new state
    public override void Execute(ObjectT objectT)
    {
        if (StateTransitionItem.TransitionActive(objectT)) return;
        if (StateTransitionItem.TransitionHealthPositive(objectT)) return;
        if (StateTransitionItem.TransitionHealthNegative(objectT)) return;
        StateTransitionItem.TransitionSpeed(objectT);
    }

    public override void Exit(ObjectT objectT)
    {
        string soundName = m_SoundNameDataFlyweight != null ? m_SoundNameDataFlyweight.Name : "";
        objectT.ObjectAudioMulti.StopPlaySound(soundName);
    }
}
