using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [SerializeField] protected GunDataFlyweight m_GunDataFlyweight;
    [SerializeField] protected ObjectShoot m_ObjectShoot;
    [SerializeField] protected AmmoContainer m_AmmoContainer;
    //[SerializeField] protected ObjectPool m_ObjectPool;
    //[Tooltip("Projectile force"), SerializeField]
    private float m_ShootVelocity = 10f;
    private float m_BulletVelocity = 10f;

    [Tooltip("End point of gun where shots appear")]
    [SerializeField]
    protected Transform m_MuzzlePosition;
    public UnityEvent GunFired;
    // [SerializeField] protected ScreenDeadZone m_ScreenDeadZone;
    [SerializeField] CharacterMovement m_CharacterMovement;
    [SerializeField] Character m_Character;
    protected bool m_IsCanShoot = true;
    public GameObject Target;
    [SerializeField] protected NameDataFlyweight m_CurrentBulletNameData;
    protected bool m_IsReloading;

    // void OnEnable()
    // {

    // }
    void OnDisable()
    {
        if (m_ObjectShoot == null) return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectShoot, "ShootVelocity").RemoveListener(BulletVelocityChanged);
    }

    void Start()
    {
        if (m_ObjectShoot == null) return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectShoot, "ShootVelocity").AddListener(BulletVelocityChanged);

        m_ShootVelocity = VariableFinder.GetVariableContainNameFromList(m_ObjectShoot.StatsData.DataNumVars, "ShootVelocity").NumVariable;

        if (!m_GunDataFlyweight)
        {
            Debug.LogWarning("GunDataFlyweight not found");
            return;
        }
        m_BulletVelocity = m_GunDataFlyweight.MuzzleVelocity + m_ShootVelocity;

        m_AmmoContainer.Initialize(m_GunDataFlyweight);
    }

    protected void BulletVelocityChanged(DataNumericalVariable variable)
    {
        m_ShootVelocity = variable.NumVariable;
        if (!m_GunDataFlyweight)
        {
            Debug.LogWarning("GunDataFlyweight not found");
            return;
        }
        m_BulletVelocity = m_GunDataFlyweight.MuzzleVelocity + m_ShootVelocity;
    }

    public virtual void ShootRotation(Projectile projectile = null)
    {
        //if (!CanShoot()) return;

        if (projectile)
        {
            projectile.Launch(m_MuzzlePosition.position, m_MuzzlePosition.rotation, m_BulletVelocity);
            GunFire();
        }
        else
        {
            Debug.LogWarning("Gun: Failed to retrieve a projectile from the object pool.");
        }
    }

    public virtual void ShootDirection(Projectile projectile = null)
    {
        //9
        //if (!CanShoot()) return;

        if (projectile)
        {
            //Debug.Log("(9)ShootDirection " + GetTime.GetCurrentTime("full-ms"));
            projectile.Launch(m_MuzzlePosition.position, m_CharacterMovement.MoveDirection, m_BulletVelocity);
            GunFire();
        }
        else
        {
            Debug.LogWarning("Gun: Failed to retrieve a projectile from the object pool.");
        }
    }

    public virtual void ShootTarget(Projectile projectile = null)
    {
        //if (!CanShoot()) return;

        if (m_Character == null || Target == null)
        {
            Debug.LogWarning("Gun: Target is not set.");
            return;
        }

        if (projectile)
        {
            projectile.Launch(m_MuzzlePosition.position, Target.transform, m_BulletVelocity);
            GunFire();
        }
        else
        {
            Debug.LogWarning("Gun: Failed to retrieve a projectile from the object pool.");
        }
    }

    public void ChangeBullet(NameDataFlyweight nameDataFlyweight)
    {
        m_CurrentBulletNameData = nameDataFlyweight;
    }

    public void Reload()
    {
        m_AmmoContainer.Reload(m_CurrentBulletNameData.Name);
        m_IsReloading = false;
    }

    private void GunFire()
    {
        m_AmmoContainer.AddMagazineAmmoCount(m_CurrentBulletNameData.Name, -1);
        //m_Cooldown = m_CooldownTime;

        if (GunFired != null)
        {
            GunFired.Invoke();
        }
        else
        {
            Debug.LogWarning("Gun: No listeners for GunFired event.");
        }
    }

    public bool CanShoot()
    {
        if (!m_IsCanShoot)
        {
            Debug.LogWarning("Gun: Cannot shoot - shooting is disabled.");
            return false;
        }

        // if (m_Cooldown > 0)
        // {
        //     Debug.LogWarning("Gun: Cannot shoot - cooldown is active.");
        //     return false;
        // }

        if (m_MuzzlePosition == null)
        {
            Debug.LogError("Gun: Muzzle position is not assigned.");
            return false;
        }

        if (m_AmmoContainer.CheckMagazineAmmoCount(m_CurrentBulletNameData.Name)) return true;
        if (!m_AmmoContainer.CheckAmmoCount(m_CurrentBulletNameData.Name)) return false;
        if (m_IsReloading)
        {
            //Debug.Log("Reloading");
            return false;
        }

        m_ObjectShoot.InitReload();
        m_IsReloading = true;

        return false;
    }
}
