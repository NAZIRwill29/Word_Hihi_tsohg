using UnityEngine;
using UnityEngine.Events;

//NOTE() - make sure only have one DataNumVars
public class ItemStrength : ObjectStrength
{
    public event UnityAction<float> OnStrengthChange;

    public override void ChangeAllVariable(object data, bool inZone)
    {
        base.ChangeAllVariable(data, inZone);
        StrengthChange();
    }

    protected override void ChangeAllVariable(object data)
    {
        base.ChangeAllVariable(data);
        StrengthChange();
    }

    public override void StrengthChange()
    {
        if (StatsData.DataNumVars.Count > 0)
            OnStrengthChange?.Invoke(StatsData.DataNumVars[0].NumVariable);
    }
}
