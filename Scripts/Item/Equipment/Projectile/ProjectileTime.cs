using UnityEngine;

public class ProjectileTime : ProjectileDuration
{
    [HideInInspector] public float launchCooldown;

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!m_Projectile.IsLaunch)
            return;
        launchCooldown -= Time.deltaTime;
        base.Update();
    }

    public override void Launch()
    {
        base.Launch();
        launchCooldown = m_Projectile.ProjectileDataFlyweight.LaunchTimeCooldown;
    }

    protected override bool DecideProjectileDuration()
    {
        if (launchCooldown < 0)
        {
            launchCooldown = m_Projectile.ProjectileDataFlyweight.LaunchTimeCooldown;
            return true;
        }
        else
            return false;
    }
}
