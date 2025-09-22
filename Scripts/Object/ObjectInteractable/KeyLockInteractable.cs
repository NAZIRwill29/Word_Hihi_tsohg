using UnityEngine;

public class KeyLockInteractable : LockInteracttable
{
    [SerializeField] private NameDataFlyweight m_KeyNameData;

    protected override bool CheckKey()
    {
        return GameManager.Instance.Inventory.CheckItemExist(m_KeyNameData.Name);
        //m_InteractManagerSO.OnInteractLocked.Invoke(m_KeyNameData.Name);
    }
}
