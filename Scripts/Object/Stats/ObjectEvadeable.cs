using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectEvadeable : ObjectStat, IEvadeable
{
    [SerializeField] protected ActionManager m_ActionManager;
    public bool IsEvade { get; set; }
    public void Evade(string name = "normal", float cost = 0)
    {
        //do evade
        Debug.Log("do evade");
        IsEvade = true;

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Defense",
            dataNumVar => dataNumVar.NumVariable--
        );
    }
}