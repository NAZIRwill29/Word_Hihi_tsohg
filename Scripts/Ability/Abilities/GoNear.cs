using UnityEngine;

[CreateAssetMenu(fileName = "GoNearAbility", menuName = "Abilities/GoNear")]
public class GoNear : Melee
{
    // Each Strategy can use custom logic. Implement the Use method in the subclasses
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        // Use method logs name, plays sound, and particle effect
        Debug.Log($"Using melee: {AbilityName}");
        if (objectT is Character character)
            character.ObjectMelee.GoNear(AbilityName, Cost);
        ThingHappen(objectT);
    }
}
