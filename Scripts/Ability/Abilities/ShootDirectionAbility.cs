using UnityEngine;

[CreateAssetMenu(fileName = "ShootDirectionAbility", menuName = "Abilities/Shoot/ShootDirection")]
public class ShootDirectionAbility : Shoot
{
    protected override void ShootAction(ObjectT objectT, ExecuteActionCommandData data, Projectile projectile)
    {
        if (objectT is Character character)
        {
            character.ObjectShoot.Shoot(AbilityName, Cost, data);
            character.ObjectShoot.Gun.ShootDirection(projectile);
        }
        ThingHappen(objectT);
    }
}
