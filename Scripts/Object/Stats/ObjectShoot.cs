using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// public class ProjectileShootData
// {
//     public string Name;
//     public UnityEvent<string, Projectile> OnShoot;
// }

public class ObjectShoot : ObjectStat, IShootable
{
    [SerializeField] protected ActionManager m_ActionManager;
    public bool IsShoot { get; set; }
    //[SerializeField] protected ProjectileShootData[] m_ProjectileShootData;
    public Gun Gun;
    [SerializeField] protected CommandDataFlyweight m_ReloadCommandDataFlyweight;

    public void InitReload()
    {
        if (m_ObjectT is Character character)
            character.ObjectAbility.DoAbility(m_ReloadCommandDataFlyweight, 50);
    }

    public void Reload()
    {
        Gun.Reload();
    }

    public void Shoot(string abilityName = "ShootRotation", float cost = 0, ExecuteActionCommandData data = null)
    {
        Debug.Log("do shoot");
        IsShoot = true;

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Shoot",
            dataNumVar => dataNumVar.NumVariable--
        );
    }
}