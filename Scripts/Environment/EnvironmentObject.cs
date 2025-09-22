using UnityEngine;

public class EnvironmentObject : MonoBehaviour, IActivable
{
    [SerializeField] private EnvironmentObjectRuntimeSetSO RuntimeSet;
    [SerializeField] private bool m_IsActive;
    public bool IsActive
    {
        get => m_IsActive;
        set => m_IsActive = value;
    }

    protected void OnEnable()
    {
        if (RuntimeSet) RuntimeSet.Add(this);
    }
    protected void OnDisable()
    {
        if (RuntimeSet) RuntimeSet.Remove(this);
    }

    public void Activate(bool isTrue)
    {
        m_IsActive = isTrue;
    }
}
