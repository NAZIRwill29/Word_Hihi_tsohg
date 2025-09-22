using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectMelee : ObjectStat, IMeleable
{
    [SerializeField] protected ActionManager m_ActionManager;
    public bool IsMelee { get; set; }
    public bool IsGoNear { get; set; }
    public void Melee(string name = "normal", float cost = 0)
    {
        //do melee
        Debug.Log("do melee");
        IsMelee = true;

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Melee",
            dataNumVar => dataNumVar.NumVariable--
        );
    }
    public void GoNear(string name = "normal", float cost = 0)
    {
        //do melee
        Debug.Log("do melee");
        IsGoNear = true;

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Melee",
            dataNumVar => dataNumVar.NumVariable--
        );
    }
}
