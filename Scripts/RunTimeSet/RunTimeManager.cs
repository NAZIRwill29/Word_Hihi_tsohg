using UnityEngine;

public class RunTimeManager : Singleton<RunTimeManager>
{
    public CollectibleRuntimeSetSO CollectibleRuntimeSetSO;
    public WeaponRuntimeSetSO WeaponRuntimeSetSO;
    public ProjectileRuntimeSetSO ProjectileRuntimeSetSO;
    public EnemyRuntimeSetSO EnemyRuntimeSetSO;
    public EnvironmentObjectRuntimeSetSO EnvironmentObjectRuntimeSetSO;
    public InteractableRunTimeSetSO InteractableRunTimeSetSO;
    public LightingRunTimeSetSO LightingRunTimeSetSO;
}
