// ----------------
// Object Stat 
// Num - Ability, LearnAbility, EquipAbility
// Bool - CanAbility
// ----------------
public interface IAbility : IStat
{
    //string name = "normal", string idName = "ability",
    public void DoAbility(CommandDataFlyweight commandDataFlyweight, int maxSize = 50, string pooledObjName = null);
}

[System.Serializable]
public class AbilityData : StatsData
{
    // [SerializeField] private ObservableList<Ability> m_Abilities = new();
    // public Dictionary<string, Ability> DictAbility = new();
    // public Dictionary<string, Ability> DictActiveAbility = new(), DictUseActiveAbility = new();
    // public Dictionary<string, Ability> DictPassiveAbility = new(), DictUsePassiveAbility = new();
}

// [System.Serializable]
// public class TriggerAbilityData : TriggerStatsData
// {
//     // [SerializeField] private ObservableList<Ability> m_Abilities = new();
//     // public Dictionary<string, Ability> DictAbility = new();
//     // public Dictionary<string, Ability> DictActiveAbility = new(), DictUseActiveAbility = new();
//     // public Dictionary<string, Ability> DictPassiveAbility = new(), DictUsePassiveAbility = new();
// }