using UnityEngine;

public class ObjectVisibility : ObjectStat, IVisibility
{
    public void VisibilityChange(bool isCanBeSeen, bool isCanBeHear)
    {
        VariableChanger.ChangeNameVariable(StatsData.DataBoolVars, "CanBeSeen",
            (name) => new DataBoolVariable { Name = name },  // Create new variable if not found
            (target) => target.IsCan = isCanBeSeen ? BoolNull.IsTrue : BoolNull.IsFalse
        );
        VariableChanger.ChangeNameVariable(StatsData.DataBoolVars, "CanBeHear",
            (name) => new DataBoolVariable { Name = name },  // Create new variable if not found
            (target) => target.IsCan = isCanBeHear ? BoolNull.IsTrue : BoolNull.IsFalse
        );
    }
}
