using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectDefense : ObjectStat, IDefense
{
    [SerializeField] protected ActionManager m_ActionManager;
    public bool IsDefend { get; set; }

    public void Defense(string name = "normal", float cost = 0)
    {
        //do defense
        Debug.Log("do defense");
        IsDefend = true;

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Defense",
            dataNumVar => dataNumVar.NumVariable--
        );
    }
}
