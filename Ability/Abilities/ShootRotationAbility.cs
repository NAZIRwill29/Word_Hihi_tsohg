using UnityEngine;

[CreateAssetMenu(fileName = "ShootRotateAbility", menuName = "Abilities/Shoot/ShootRotate")]
public class ShootRotationAbility : Shoot
{
    protected override void ShootAction(ObjectT objectT, ExecuteActionCommandData data, Projectile projectile)
    {
        if (objectT is Character character)
        {
            character.ObjectShoot.Shoot(AbilityName, Cost, data);
            character.ObjectShoot.Gun.ShootRotation(projectile);
        }
        ThingHappen(objectT);
    }
}
