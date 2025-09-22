using UnityEngine;

[CreateAssetMenu(fileName = "AirSupportAbility", menuName = "Abilities/AirSupport")]
public class AirSupport : Ability
{
    // Each Strategy can use custom logic. They can be unique and varied; 
    // execute any intended gameplay logic in the overridden Use method
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT);

        // Implement additional Air Support logic here


    }
}