using UnityEngine;

[CreateAssetMenu(fileName = "ShootAbility", menuName = "Abilities/Shoot/Shoot")]
public class Shoot : Ability
{
    // Each Strategy can use custom logic. Implement the Use method in the subclasses
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        //8
        // Use method logs name, plays sound, and particle effect
        //Debug.Log($"Using shoot: {AbilityName} {data.Poolable}");
        //Debug.Log("(8)Use " + GetTime.GetCurrentTime("full-ms"));

        if (data?.Poolable is Projectile projectile)
        {
            // Use reflection to find and call the method dynamically
            //var method = typeof(Gun).GetMethod(m_AbilityName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (objectT is Character character)
            {
                if (!character.ObjectShoot.Gun.CanShoot())
                {
                    Debug.Log("Cannot shoot");
                    projectile.Deactivate();
                    return;
                }
                ShootAction(objectT, data, projectile);
            }
        }
        else
            Debug.Log("Poolable is not projectile");
    }

    protected virtual void ShootAction(ObjectT objectT, ExecuteActionCommandData data, Projectile projectile)
    {
        if (objectT is Character character)
            character.ObjectShoot.Shoot(AbilityName, Cost, data);
        //method.Invoke(objectT.ObjectShoot.Gun, new object[] { projectile });
        ThingHappen(objectT);
    }
}
