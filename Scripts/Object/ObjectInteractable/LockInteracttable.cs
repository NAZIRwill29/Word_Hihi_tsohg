using UnityEngine;

public class LockInteracttable : SwitchInteractable
{
    protected override void Interact()
    {
        if (!m_IsCheckInteractKey) return;
        if (CheckKey())
            ChangePhase();
    }

    protected virtual bool CheckKey()
    {
        return false;
    }

    protected void ChangePhase()
    {
        if (m_PhaseNum == 0) //open
        {
            m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseOneNameData.Name);
            m_PhaseNum = 1;
        }
        else //close
        {
            m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseZeroNameData.Name);
            m_PhaseNum = 0;
        }
        m_ObjectSR.sprite = m_ObjectImages[m_PhaseNum];
    }
}
