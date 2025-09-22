using UnityEngine;

public class MiniGameSO : ScriptableObject
{
    public NameDataFlyweight NameData;
    // protected bool m_HasInitialized;
    protected bool m_IsActive;
    protected WordSystem m_WordSystem;
    [Tooltip("must be the longest"), Range(20, 150)] public float Duration = 90;
    public bool HasDuration;
    [SerializeField] protected float ShakeIntensity = 0.5f;

    public virtual void Initialize(WordSystem wordSystem)
    {
        // if (m_HasInitialized) return;
        // m_HasInitialized = true;
        m_IsActive = true;
        m_WordSystem = wordSystem;
    }

    public virtual void StartMiniGame()
    {

    }

    public virtual void DoUpdate()
    {
        if (!m_IsActive) return;
    }

    protected virtual void DoPunishment()
    {
        CameraManager.Instance.CameraShake.Shake(ShakeIntensity);
    }

    protected virtual void DoReward()
    {
    }

    public virtual void OnTyping(string word)
    {
    }

    public virtual void OnCompleteWord(string word)
    {
    }

    public virtual void OnSuccessWord(string word)
    {
    }

    public virtual void OnFailedWord(string word)
    {
    }

    public virtual void OnSuccessWordNotDuplicate(string word)
    {
    }

    public virtual void OnFailedWordNotDuplicate(string word)
    {
    }

    public virtual void Exit(bool isByPlayer)
    {
        m_IsActive = false;
    }
}
