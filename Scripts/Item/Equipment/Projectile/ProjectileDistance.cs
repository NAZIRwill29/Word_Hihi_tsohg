using UnityEngine;

public class ProjectileDistance : ProjectileDuration
{
    [HideInInspector] public Vector2 originPos;

    public override void Launch()
    {
        base.Launch();
        originPos = gameObject.transform.position;
    }
    protected override bool DecideProjectileDuration()
    {
        return Vector2.Distance(originPos, gameObject.transform.position) > m_Projectile.ProjectileDataFlyweight.Distance;
    }
}
