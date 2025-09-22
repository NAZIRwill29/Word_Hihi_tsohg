using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torchlight : MonoBehaviour
{
    public ObjectT ObjectT;
    private bool m_IsOn;

    public void TurnTorchLight()
    {
        m_IsOn = !m_IsOn;
        ObjectT.Animator.SetBool("Light", m_IsOn);
    }

    public void TurnTorchLight(bool isOn)
    {
        m_IsOn = isOn;
        ObjectT.Animator.SetBool("Light", m_IsOn);
    }
}
