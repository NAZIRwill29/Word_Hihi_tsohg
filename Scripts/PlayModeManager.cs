using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayModeManager : MonoBehaviour
{
    public ListWrapper<PlayModeObjWithName> PlayModeObjs;
    [SerializeField] private NameDataFlyweight m_NormalListenerNameData;
    [SerializeField] private NameDataFlyweight m_CombatListenerNameData;
    public NameDataFlyweight NormalListenerNameData { get => m_NormalListenerNameData; }
    private int m_PlayModeCurrentIndex;
    public string PlayModeCurrentName { get; private set; }
    public UnityEvent<string> OnPlayModeChange;

    void Awake()
    {
        for (int i = 1; i < PlayModeObjs.Items.Count; i++)
        {
            PlayModeObjs.Items[i].EnableGameObj(false);
        }
    }

    public void ChangePlayMode(string name)
    {
        if (PlayModeObjs.TryGetIndex(x => x.NameData.Name == name, out int index))
        {
            int prevIndex = m_PlayModeCurrentIndex;
            bool isSame = prevIndex == index;
            PlayModeCurrentName = name;
            m_PlayModeCurrentIndex = index;
            if (!isSame)
                PlayModeObjs.Items[prevIndex].EnableGameObj(false);
            PlayModeObjs.Items[m_PlayModeCurrentIndex].EnableGameObj(true);
            ManageObject(PlayModeObjs.Items[m_PlayModeCurrentIndex].NameData != m_CombatListenerNameData);
            OnPlayModeChange?.Invoke(PlayModeObjs.Items[m_PlayModeCurrentIndex].NameData.Name);
        }
    }

    void ManageObject(bool isTrue)
    {
        // Make temporary copies of each list before iterating
        // var enemies = new List<Enemy2D>(RunTimeManager.Instance.EnemyRuntimeSetSO.Items);
        // var environmentObjects = new List<EnvironmentObject>(RunTimeManager.Instance.EnvironmentObjectRuntimeSetSO.Items);
        // var collectibles = new List<Collectible>(RunTimeManager.Instance.CollectibleRuntimeSetSO.Items);
        // var projectiles = new List<Projectile>(RunTimeManager.Instance.ProjectileRuntimeSetSO.Items);
        // var weapons = new List<Weapon>(RunTimeManager.Instance.WeaponRuntimeSetSO.Items);

        foreach (var item in RunTimeManager.Instance.EnemyRuntimeSetSO.Items)
        {
            if (item is IActivable activable)
                activable.Activate(isTrue);
        }
        foreach (var item in RunTimeManager.Instance.EnvironmentObjectRuntimeSetSO.Items)
        {
            if (item is IActivable activable)
                activable.Activate(isTrue);
        }
        foreach (var item in RunTimeManager.Instance.CollectibleRuntimeSetSO.Items)
        {
            if (item is IActivable activable)
                activable.Activate(isTrue);
        }
        foreach (var item in RunTimeManager.Instance.ProjectileRuntimeSetSO.Items)
        {
            if (item is IActivable activable)
                activable.Activate(isTrue);
        }
        foreach (var item in RunTimeManager.Instance.WeaponRuntimeSetSO.Items)
        {
            if (item is IActivable activable)
                activable.Activate(isTrue);
        }
    }
}
