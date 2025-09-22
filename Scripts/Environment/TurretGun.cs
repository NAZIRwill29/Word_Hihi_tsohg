using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

/// <summary>
/// This represents a gun that shoots projectiles from an ObjectPool.
/// </summary>
public class TurretGun : Gun
{
    void FixedUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        // if (m_ScreenDeadZone.IsMouseInDeadZone())
        //     return;

        // Shoot when the Fire1 button is pressed and cooldown time has passed
        if (Input.GetButton("Fire1") && CanShoot())
        {
            ShootRotation();
        }
    }
}
