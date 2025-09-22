using System;
using UnityEngine;

[System.Serializable]
public class BulletData
{
    public BulletDataFlyweight BulletDataFlyweight;
    public string BulletName => BulletDataFlyweight?.BulletNameData?.Name ?? "Unknown";
    public int AmmoCount;
    public int AmmoCountMax = 100;
    public int MagazineCapacity { get; set; }
    public int MagazineAmmoCount;

    public void AddAmmoCount(int num)
    {
        AmmoCount = Math.Clamp(AmmoCount + num, 0, AmmoCountMax);
    }

    public void AddMagazineAmmoCount(int num)
    {
        MagazineAmmoCount = Math.Clamp(MagazineAmmoCount + num, 0, MagazineCapacity);
    }
}

public class AmmoContainer : MonoBehaviour
{
    [SerializeField] protected ListWrapper<BulletData> m_BulletDataList;

    public void Initialize(GunDataFlyweight gunDataFlyweight)
    {
        foreach (var item in m_BulletDataList.Items)
        {
            if (item.BulletDataFlyweight != null)
            {
                item.MagazineCapacity = gunDataFlyweight.MagazineCapacity / item.BulletDataFlyweight.BulletSize;
            }
        }
    }

    private BulletData GetBulletData(string bulletName)
    {
        for (int i = 0; i < m_BulletDataList.Items.Count; i++)
        {
            if (m_BulletDataList.Items[i].BulletName == bulletName)
                return m_BulletDataList.Items[i];
        }
        return null;
    }

    public bool CheckMagazineAmmoCount(string bulletName)
    {
        BulletData bulletData = GetBulletData(bulletName);
        if (bulletData == null)
        {
            //Debug.Log($"Bullet '{bulletName}' not found");
            return false;
        }

        if (bulletData.MagazineAmmoCount > 0) return true;

        //Debug.Log("Magazine Ammo empty");
        return false;
    }

    public bool CheckAmmoCount(string bulletName)
    {
        BulletData bulletData = GetBulletData(bulletName);
        if (bulletData == null)
        {
            //Debug.Log($"Bullet '{bulletName}' not found");
            return false;
        }

        if (bulletData.AmmoCount > 0) return true;

        //Debug.Log("Ammo empty");
        return false;
    }

    public void Reload(string bulletName)
    {
        BulletData bulletData = GetBulletData(bulletName);
        if (bulletData == null)
        {
            //Debug.Log($"Bullet '{bulletName}' not found");
            return;
        }

        if (bulletData.AmmoCount <= 0)
        {
            //Debug.Log("Ammo empty");
            return;
        }

        int spaceLeft = bulletData.MagazineCapacity - bulletData.MagazineAmmoCount;
        if (spaceLeft <= 0)
        {
            //Debug.Log("Magazine is already full");
            return;
        }

        int reloadNum = Math.Min(bulletData.AmmoCount, spaceLeft);
        bulletData.AddAmmoCount(-reloadNum);
        bulletData.AddMagazineAmmoCount(reloadNum);
    }

    public void AddAmmoCount(int num)
    {
        AddAmmoCount("Projectile Player A", num);
    }

    public void AddAmmoCount(string bulletName, int num)
    {
        BulletData bulletData = GetBulletData(bulletName);
        if (bulletData != null)
            bulletData.AddAmmoCount(num);
    }

    public void AddMagazineAmmoCount(string bulletName, int num)
    {
        BulletData bulletData = GetBulletData(bulletName);
        if (bulletData != null)
            bulletData.AddMagazineAmmoCount(num);
    }
}
