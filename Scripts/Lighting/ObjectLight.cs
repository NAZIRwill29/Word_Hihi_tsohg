using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectLight : MonoBehaviour
{
    protected Light2D m_Light2D;
    [SerializeField] protected float m_LightIntensityOn = 0.75f;
    [SerializeField] protected LightingRunTimeSetSO m_RuntimeSet;

    protected void OnEnable()
    {
        if (m_RuntimeSet) m_RuntimeSet.Add(this);
    }
    protected void OnDisable()
    {
        if (m_RuntimeSet) m_RuntimeSet.Remove(this);
    }

    void Awake()
    {
        m_Light2D = GetComponent<Light2D>();
    }

    public void LightOn(bool isLight)
    {
        m_Light2D.intensity = isLight ? m_LightIntensityOn : 0;
    }
}
