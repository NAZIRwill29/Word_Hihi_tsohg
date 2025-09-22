using System.Linq;
using UnityEngine;

public class PlayerMelee : ObjectMelee
{
    [SerializeField] protected InputManagerSO m_InputManagerSO;
    [SerializeField] protected CommandDataFlyweight m_MeleeCommandDataFlyweight;
    [SerializeField] protected NameDataFlyweight m_InputNameDataFlyweight;

    protected void OnEnable()
    {
        if (!m_InputManagerSO || !m_InputNameDataFlyweight) return;
        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
            inputDetail.Action.AddListener(InitMelee);
    }

    protected void OnDisable()
    {
        if (!m_InputManagerSO || !m_InputNameDataFlyweight) return;
        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
            inputDetail.Action.RemoveListener(InitMelee);
    }

    private void InitMelee()
    {
        if (m_ObjectT is Character character)
            character.ObjectAbility.DoAbility(m_MeleeCommandDataFlyweight);
    }
}
