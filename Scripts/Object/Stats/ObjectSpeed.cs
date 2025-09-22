using UnityEngine;

public class ObjectSpeed : ObjectStat, ISpeed
{
    public bool IsStandBy { get; set; }
    public void SpeedChangeByPass(float addNum)
    {
        VariableChanger.ChangeNameVariable(StatsData.DataNumVars, "Speed",
            (name) => new DataNumericalVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.NumVariable += addNum;
            }
        );
    }

    public void CanWalkChange(bool isTrue)
    {
        VariableChanger.ChangeNameVariable(StatsData.DataBoolVars, "CanWalk",
            (name) => new DataBoolVariable { Name = name },  // Create new variable if not found
            (target) =>
            {
                target.IsCan = isTrue ? BoolNull.IsTrue : BoolNull.IsFalse;
            }
        );
    }
}