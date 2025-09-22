using System.Collections.Generic;
using UnityEngine;

public class LockingMechanic : MonoBehaviour
{
    [SerializeField] private NormalUI m_NormalUI;
    [SerializeField] private MiniGameUI m_MiniGameUI;
    private bool m_IsInUnlocking;
    public MiniGameLockInteractable MiniGameLockInteractable { get; set; }
    public int StartLockpickHealth = 3;
    public float LockpickHealth { get; private set; }
    [SerializeField] private NameDataFlyweight m_LockpickName;
    public List<string> Words;
    public List<string> WordFitConditions;
    [SerializeField] private ActionControllerSO m_ActionControllerSO;

    void Start()
    {
        LockpickHealth = StartLockpickHealth;
    }

    public void StartLockpickMiniGame(MiniGameLockInteractable miniGameLockInteractable)
    {
        if (m_IsInUnlocking) return;
        m_IsInUnlocking = true;
        MiniGameLockInteractable = miniGameLockInteractable;
        GameManager.Instance.ChangePlayMode("MiniGame");
        GameManager.Instance.player2D.ObjectSpeed.CanWalkChange(false);
    }

    public void ReduceLockpickHealth()
    {
        LockpickHealth--;
        m_MiniGameUI.UpdateLockUI();
        if (LockpickHealth <= 0)
        {
            ReduceLockpick();
            LockpickHealth = StartLockpickHealth;
        }
    }

    public void ReduceLockpick()
    {
        Item item = GameManager.Instance.Inventory.Items.Find(x => x.ItemDataFlyweight.NameData.Name == m_LockpickName.Name);
        if (item != null)
        {
            item.quantity--;
        }
        else
        {
            m_NormalUI.OnShowPopUp("Failed", item.ItemDataFlyweight.Icon, "0", "No Lockpick in inventory");
            GameManager.Instance.NormalSystem.NegativeActionControllerSO = m_ActionControllerSO;
        }
    }
}
