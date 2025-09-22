using UnityEngine;

public class ObjectStrength : ObjectStat, IStrength
{
    public virtual void StrengthChange()
    {
        //OnStrengthChange?.Invoke(strengthNum);
    }

    protected override void Start()
    {
        base.Start();
        StrengthData strengthData = new();
    }
}
