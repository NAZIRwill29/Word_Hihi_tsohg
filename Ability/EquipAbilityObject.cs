using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipAbilityObject : MonoBehaviour, ICommandExecuteUndoable
{
    [SerializeField] private ObjectT m_ObjectT;
    [SerializeField] private CommandDataFlyweight m_CommandDataFlyweight;
    [SerializeField] private AbilityManager m_AbilityManager;
    public ObservableListWrapper<Ability> EquipAbilities = new();
    [SerializeField] private ObjectAbility m_ObjectAbility;
    [SerializeField] private LearnAbilityObject m_LearnAbilityObject;

    //public Dictionary<string, Ability> DictEquipAbility { get; private set; } = new();
    public Dictionary<string, Ability> DictEquipActiveAbility { get; private set; } = new();
    public Dictionary<string, Ability> DictEquipPassiveAbility { get; private set; } = new();

    void Start()
    {
        if (m_LearnAbilityObject == null)
        {
            Debug.LogError("m_LearnAbilityObject is not assigned in EquipAbilityObject!");
            return;
        }

        EquipAbilities.OnListChanged += UpdateEquipDictionaries;
        m_LearnAbilityObject.OnListChanged += UpdateEquipDictionaries;

        //UpdateEquipDictionaries();
    }

    void OnDisable()
    {
        if (m_LearnAbilityObject != null)
            m_LearnAbilityObject.OnListChanged -= UpdateEquipDictionaries;

        EquipAbilities.OnListChanged -= UpdateEquipDictionaries;
    }

    public void EquipAbility(string name = "normal")
    {
        EquipAbility(m_CommandDataFlyweight);
    }

    public void EquipAbility(CommandDataFlyweight commandDataFlyweight)
    {
        if (m_LearnAbilityObject == null || !m_LearnAbilityObject.Abilities.Find(x => x.AbilityName == commandDataFlyweight.CmdName))
        {
            Debug.LogWarning($"EquipAbility: Cannot equip ability '{commandDataFlyweight.CmdName}', it is not learned.");
            return;
        }

        ICommandUndoable command = new EquipAbilityCommand(this, m_ObjectT.ObjectId, commandDataFlyweight.CmdName, commandDataFlyweight.IdName);
        CommandInvoker.ExecuteCommand(command);
    }

    public void UndoEquipAbility(string name = "normal")
    {
        UndoEquipAbility(m_CommandDataFlyweight);
    }

    public void UndoEquipAbility(CommandDataFlyweight commandDataFlyweight)
    {
        //&& m_LearnAbilityObject.Abilities.Find(x => x.AbilityName == name)
        if (m_LearnAbilityObject != null)
            CommandInvoker.UndoCommand(m_ObjectT.ObjectId.ToString(), commandDataFlyweight.IdName);
        else
            Debug.LogWarning($"EquipAbility: Cannot undo equip ability '{commandDataFlyweight.CmdName}', no reference LearnAbilityObject");
    }

    public void RedoEquipAbility(string name = "normal")
    {
        RedoEquipAbility(m_CommandDataFlyweight);
    }

    public void RedoEquipAbility(CommandDataFlyweight commandDataFlyweight)
    {
        if (m_LearnAbilityObject != null && m_LearnAbilityObject.Abilities.Find(x => x.AbilityName == commandDataFlyweight.CmdName))
            CommandInvoker.RedoCommand(m_ObjectT.ObjectId.ToString(), commandDataFlyweight.IdName);
        else
            Debug.LogWarning($"EquipAbility: Cannot redo equip ability '{commandDataFlyweight.CmdName}', it is not learned.");
    }

    #region ICommand Methods

    public void ExecuteUndoable(string name = "normal")
    {
        if (m_AbilityManager?.DictAbilityCollection == null)
        {
            Debug.LogWarning("ExecuteUndoable: GameManager or AbilityManager is null.");
            return;
        }

        if (m_AbilityManager.DictAbilityCollection.TryGetValue(name, out Ability ability))
        {
            Debug.Log($"Equip Ability: {name}");

            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectAbility,
                m_ObjectAbility.StatsData.DataNumVars,
                "Ability",
                dataNumVar => dataNumVar.NumVariable++
            );

            AddEquipAbility(name);
        }
    }

    public void Undo(string name = "normal")
    {
        if (EquipAbilities.Find(x => x.AbilityName == name))
        {
            Debug.Log($"Undo equipped Ability: {name}");

            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectAbility,
                m_ObjectAbility.StatsData.DataNumVars,
                "Ability",
                dataNumVar => dataNumVar.NumVariable--
            );

            if (EquipAbilities.TryGetValue(x => x.AbilityName == name, out Ability ability))
                EquipAbilities.Remove(ability);
        }
        else
        {
            Debug.LogWarning($"Undo: Ability '{name}' not found in equipped abilities.");
        }
    }

    public void Redo(string name = "normal")
    {
        // if (m_LearnAbilityObject?.Abilities.Find(x => x.AbilityName == name))
        // {
        Debug.Log($"Redo Use Ability: {name}");

        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            m_ObjectAbility,
            m_ObjectAbility.StatsData.DataNumVars,
            "Ability",
            dataNumVar => dataNumVar.NumVariable++
        );

        AddEquipAbility(name);
        // }
        // else
        // {
        //     Debug.LogWarning($"Redo: Ability '{name}' not found in learned abilities.");
        // }
    }

    #endregion

    private void AddEquipAbility(string name)
    {
        if (!EquipAbilities.Find(x => x.AbilityName == name))
            EquipAbilities.Add(m_LearnAbilityObject.Abilities.Find(x => x.AbilityName == name));
    }

    public void UpdateEquipDictionaries()
    {
        //DictEquipAbility.Clear();
        DictEquipActiveAbility.Clear();
        DictEquipPassiveAbility.Clear();

        if (m_LearnAbilityObject == null)
        {
            Debug.LogWarning("UpdateEquipDictionaries: m_LearnAbilityObject is null.");
            return;
        }

        foreach (Ability ability in EquipAbilities.Items)
        {
            // if (m_LearnAbilityObject.Abilities.ContainsKey(ability))
            // {
            //DictEquipAbility[ability.AbilityName] = ability;

            if (ability.IsPassiveAbility) // Fixed typo from IsPassvieAbility
                DictEquipPassiveAbility[ability.AbilityName] = ability;
            else
                DictEquipActiveAbility[ability.AbilityName] = ability;
            // }
            // else
            // {
            //     Debug.LogWarning($"UpdateEquipDictionaries: Ability '{ability.AbilityName}' not found in learned abilities.");
            // }
        }
    }
}
