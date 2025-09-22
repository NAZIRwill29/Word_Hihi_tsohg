using UnityEngine;

[CreateAssetMenu(fileName = "DefendAbility", menuName = "Abilities/Defend")]
public class Defend : Ability
{
    // Each Strategy can use custom logic. Implement the Use method in the subclasses
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        // Use method logs name, plays sound, and particle effect
        Debug.Log($"Using defense: {AbilityName}");
        if (objectT is Character character)
            character.ObjectDefense.Defense(AbilityName, Cost);
        ThingHappen(objectT);
    }
}
