using UnityEngine;

[CreateAssetMenu(fileName = "ShootTargetAbility", menuName = "Abilities/Shoot/ShootTarget")]
public class ShootTargetAbility : Shoot
{
    protected override void ShootAction(ObjectT objectT, ExecuteActionCommandData data, Projectile projectile)
    {
        if (objectT is Character character)
        {
            character.ObjectShoot.Shoot(AbilityName, Cost, data);
            character.ObjectShoot.Gun.ShootTarget(projectile);
        }
        ThingHappen(objectT);
    }
}
