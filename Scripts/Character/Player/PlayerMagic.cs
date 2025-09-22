using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMagic : ObjectMagic
{
    [SerializeField] protected InputManagerSO m_InputManagerSO;
    [SerializeField] protected CommandPooledObjDataFlyweight m_CommandPooledObjDataFlyweight;
    [SerializeField] protected NameDataFlyweight m_InputNameDataFlyweight;

    protected void OnEnable()
    {
        if (!m_InputManagerSO || !m_InputNameDataFlyweight) return;
        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
            inputDetail.Action.AddListener(InitMagic);
    }

    protected void OnDisable()
    {
        if (!m_InputManagerSO || !m_InputNameDataFlyweight) return;
        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
            inputDetail.Action.RemoveListener(InitMagic);
    }

    private void InitMagic()
    {
        if (m_ObjectT is Character character)
            character.ObjectAbility.DoAbility(m_CommandPooledObjDataFlyweight, 50, m_CommandPooledObjDataFlyweight.PooledObjNameDataFlyweight.Name);
    }
}
