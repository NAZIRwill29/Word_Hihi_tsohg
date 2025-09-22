using UnityEngine;

public class MiniGameLockInteractable : LockInteracttable
{
    public int LockHealth { get; set; }
    public int StartLockHealth = 0;
    [SerializeField] private MiniGameSO m_MiniGameSO;

    void Start()
    {
        LockHealth = StartLockHealth;
    }

    protected override void Interact()
    {
        GameManager.Instance.LockingMechanic.StartLockpickMiniGame(this);
        GameManager.Instance.MiniGameManager.ChangeMiniGame(m_MiniGameSO);
        //if (!m_IsCheckInteractKey) return;
    }

    public void Unlock()
    {
        ChangePhase();
    }
}
