using UnityEngine;

[CreateAssetMenu(fileName = "EvadeAbility", menuName = "Abilities/Evade")]
public class Evade : Ability
{
    // Each Strategy can use custom logic. Implement the Use method in the subclasses
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        // Use method logs name, plays sound, and particle effect
        Debug.Log($"Using melee: {AbilityName}");
        if (objectT is Character character)
            character.ObjectEvadeable.Evade(AbilityName, Cost);
        ThingHappen(objectT);
    }
}
