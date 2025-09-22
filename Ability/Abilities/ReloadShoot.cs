using UnityEngine;

[CreateAssetMenu(fileName = "ReloadShootAbility", menuName = "Abilities/Shoot/ReloadShoot")]
public class ReloadShoot : Ability
{
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        //8
        // Use method logs name, plays sound, and particle effect
        Debug.Log($"Using ReloadShoot: {AbilityName}");
        //Debug.Log("(8)Use "a + GetTime.GetCurrentTime("full-ms"));

        //objectT.ObjectShoot.Shoot(m_AbilityName, data);
        if (objectT is Character character)
            character.ObjectShoot.Gun.Reload();
        ThingHappen(objectT);
    }
}
