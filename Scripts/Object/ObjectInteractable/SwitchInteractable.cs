using UnityEngine;

public class SwitchInteractable : ObjectInteractable
{
    [SerializeField] protected Sprite[] m_ObjectImages;
    [SerializeField] protected SpriteRenderer m_ObjectSR;

    protected override void TriggerEffect(Collision2D other)
    {
        TriggerEffect(other.collider);
    }

    protected override void TriggerEffect(Collider2D other)
    {
        m_InteractManagerSO.OnCanInteract.Invoke(
            m_PhaseNum == 0 ? m_InteractPhaseZeroNameData.Name : m_InteractPhaseOneNameData.Name,
            true
        );
        m_IsCheckInteractKey = true;
    }

    protected override void TriggerExit(Collider2D other)
    {
        m_InteractManagerSO.OnCanInteract.Invoke(
            m_PhaseNum == 0 ? m_InteractPhaseZeroNameData.Name : m_InteractPhaseOneNameData.Name,
            false
        );
        m_IsCheckInteractKey = false;
    }

    protected override void Interact()
    {
        if (!m_IsCheckInteractKey) return;
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
