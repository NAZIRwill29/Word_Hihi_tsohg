using UnityEngine;

public class ProjectileDuration : MonoBehaviour
{
    protected Projectile m_Projectile;

    void Start()
    {
        m_Projectile = GetComponent<Projectile>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!m_Projectile.IsLaunch)
            return;
        if (DecideProjectileDuration())
        {
            m_Projectile.IsLaunch = false;
            m_Projectile.Deactivate();
        }
    }

    public virtual void Launch()
    {

    }

    protected virtual bool DecideProjectileDuration()
    {
        return true;
        // switch (projectileDurType)
        // {
        //     case ProjectileDurType.distance:
        //         return Vector3.Distance(originPos, gameObject.transform.position) > distance;
        //     case ProjectileDurType.timerDist:
        //         return Vector3.Distance(originPos, gameObject.transform.position) > distance || launchCooldown < 0;
        //     //timer
        //     default:
        //         return launchCooldown < 0;
        // }
    }
}
