using UnityEngine;
using UnityEngine.Events;

public class NatureElementController : MonoBehaviour
{
    // fire, water, earth, metal, wood, wind, ice, thunder, light, dark
    [SerializeField] protected NatureElement m_NatureElement;
    //public NatureElementEffectSO NatureElementEffectSO;

    protected void OnTrigger(Collider2D other) // Keeping the name as per your request
    {
        if (other == null) return;
        if (m_NatureElement == null) return;

        NatureElementController otherController = other.GetComponent<NatureElementController>();
        if (otherController == null) return;

        NatureElement otherNatureElement = otherController.m_NatureElement;
        if (otherNatureElement == null) return;

        NatureElementResult natureElementResult = m_NatureElement.Confront(otherNatureElement);

        //if (NatureElementEffectSO == null) return; // Ensure the scriptable object is assigned

        if (natureElementResult.IsBenefit) BenefitEffect();
        if (natureElementResult.IsSupport) SupportEffect();
        if (natureElementResult.IsStrength) StrengthEffect();
        if (natureElementResult.IsWeakness) WeaknessEffect();
    }

    protected virtual void BenefitEffect()
    {
    }

    protected virtual void SupportEffect()
    {
    }

    protected virtual void StrengthEffect()
    {
    }

    protected virtual void WeaknessEffect()
    {
    }
}
