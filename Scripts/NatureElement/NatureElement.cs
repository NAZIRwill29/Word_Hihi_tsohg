using UnityEngine;

public class NatureElementResult
{
    public bool IsBenefit, IsSupport, IsStrength, IsWeakness;
}

[CreateAssetMenu(fileName = "NatureElement", menuName = "NatureElement/NatureElement")]
public class NatureElement : ScriptableObject
{
    [SerializeField] private NatureElement[] m_BenefitFrom;
    [SerializeField] private NatureElement[] m_SupportTo;
    [SerializeField] private NatureElement[] m_Strength;
    [SerializeField] private NatureElement[] m_Weakness;

    public NatureElementResult Confront(NatureElement other)
    {
        NatureElementResult result = new();
        if (IsBenefit(other))
        {
            result.IsBenefit = true;
            Debug.Log("this element benefit from " + other);
        }
        if (IsSupport(other))
        {
            result.IsSupport = true;
            Debug.Log("this element support " + other);
        }
        if (IsStrength(other))
        {
            result.IsStrength = true;
            Debug.Log("this element strong against " + other);
        }
        else if (IsWeakness(other))
        {
            result.IsWeakness = true;
            Debug.Log("this element weak against " + other);
        }
        // else
        // {
        //     Debug.Log("this element has no effect against " + other);
        // }

        return result;
    }

    private bool IsBenefit(NatureElement other)
    {
        for (int i = 0; i < m_BenefitFrom.Length; i++)
        {
            if (other == m_BenefitFrom[i]) return true;
        }
        return false;
    }

    private bool IsSupport(NatureElement other)
    {
        for (int i = 0; i < m_SupportTo.Length; i++)
        {
            if (other == m_SupportTo[i]) return true;
        }
        return false;
    }

    private bool IsStrength(NatureElement other)
    {
        for (int i = 0; i < m_Strength.Length; i++)
        {
            if (other == m_Strength[i]) return true;
        }
        return false;
    }

    private bool IsWeakness(NatureElement other)
    {
        for (int i = 0; i < m_Weakness.Length; i++)
        {
            if (other == m_Weakness[i]) return true;
        }
        return false;
    }
}
