using UnityEngine;

public class TestSwitchable : MonoBehaviour, ISwitchable
{
    private SpriteRenderer m_SriteRenderer;
    // ISwitchable active state
    private bool m_IsActive;
    public bool IsActive => m_IsActive;

    void Awake()
    {
        m_SriteRenderer = GetComponent<SpriteRenderer>();
        m_SriteRenderer.enabled = false;
    }

    // Enabling physics and mark it as active.
    public void Activate()
    {
        m_SriteRenderer.enabled = true;
        m_IsActive = true;
        Debug.Log("The test switchable is active.");
    }

    // Deactivates the trap and marks it as inactive.
    public void Deactivate()
    {
        m_SriteRenderer.enabled = false;
        m_IsActive = false;
        Debug.Log("The test switchable is reset.");
    }
}