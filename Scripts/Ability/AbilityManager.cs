using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityManager", menuName = "Manager/AbilityManager")]
public class AbilityManager : ScriptableObject
{
    [SerializeField] private List<Ability> AbilityCollection = new();

    public Dictionary<string, Ability> DictAbilityCollection { get; private set; } = new();
    public Dictionary<string, Ability> DictActiveAbilityCollection { get; private set; } = new();
    public Dictionary<string, Ability> DictPassiveAbilityCollection { get; private set; } = new();

    public void Initialize()
    {
        if (AbilityCollection == null || AbilityCollection.Count == 0)
        {
            Debug.LogWarning("AbilityManager: AbilityCollection is empty or null. Initialization skipped.");
            return;
        }

        DictAbilityCollection.Clear();
        DictActiveAbilityCollection.Clear();
        DictPassiveAbilityCollection.Clear();

        foreach (var item in AbilityCollection)
        {
            if (item == null)
            {
                Debug.LogWarning("AbilityManager: Null ability found in AbilityCollection, skipping.");
                continue;
            }

            DictAbilityCollection[item.AbilityName] = item;

            if (item.IsPassiveAbility)
                DictPassiveAbilityCollection[item.AbilityName] = item;
            else
                DictActiveAbilityCollection[item.AbilityName] = item;
        }

        Debug.Log("AbilityManager initialized successfully.");
    }
}
