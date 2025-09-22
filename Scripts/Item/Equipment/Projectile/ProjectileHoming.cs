using UnityEngine;
using UnityEngine.Pool;

public class ProjectileHoming : Projectile
{
    Transform targetHoming;

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsLaunch)
            return;
        transform.position = Vector2.MoveTowards(
            this.transform.position,
            targetHoming.position,
            m_Force * Time.deltaTime
        );
    }

    //launch homing
    public override void Launch(Vector2 pos, Transform target, float velocity)
    {
        /*
        The function calls AddForce on the projectile’s Rigidbody component; the calculation for this here is 
        the direction variable multiplied by the force variable.
        When the force is applied to the Rigidbody component, the physics engine will apply that force and 
        direction to move the Projectile GameObject every frame.
        */
        IsLaunch = true;
        foreach (var item in m_ProjectileDurations)
        {
            item.Launch();
        }
        m_OriginPos = pos;
        targetHoming = target;
    }
}
