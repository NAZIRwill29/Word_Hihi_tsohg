using UnityEngine;

public class PlayerShoot : ObjectShoot
{
    [SerializeField] protected InputManagerSO m_InputManagerSO;
    [SerializeField] protected CommandPooledObjDataFlyweight m_ShootCommandPooledObjDataFlyweight;
    [SerializeField] protected NameDataFlyweight m_ShootInputNameDataFlyweight;

    protected void OnEnable()
    {
        if (!m_InputManagerSO || !m_ShootInputNameDataFlyweight) return;
        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_ShootInputNameDataFlyweight.Name, out InputDetail inputDetail))
            inputDetail.Action.AddListener(InitShoot);
    }

    protected void OnDisable()
    {
        if (!m_InputManagerSO || !m_ShootInputNameDataFlyweight) return;
        if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_ShootInputNameDataFlyweight.Name, out InputDetail inputDetail))
            inputDetail.Action.RemoveListener(InitShoot);
    }

    private void InitShoot()
    {
        //1
        //Debug.Log("(1)Shoot " + GetTime.GetCurrentTime("full-ms")); "ShootDirection", "shoot" "Projectile Player A"
        if (m_ObjectT is Character character)
            character.ObjectAbility.DoAbility(m_ShootCommandPooledObjDataFlyweight, 50, m_ShootCommandPooledObjDataFlyweight.PooledObjNameDataFlyweight.Name);
        //ObjectShoot.Shoot("Projectile Player A");
    }
}