using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LearnAbilityObject : MonoBehaviour, ICommandExecuteUndoable
{
    [SerializeField] private ObjectT m_ObjectT;
    [SerializeField] private CommandDataFlyweight m_CommandDataFlyweight;
    [SerializeField] private EquipAbilityObject m_EquipAbilityObject;
    [SerializeField] private AbilityManager m_AbilityManager;
    public ObservableListWrapper<Ability> Abilities = new();
    [SerializeField] private ObjectAbility m_ObjectAbility;

    //public Dictionary<string, Ability> DictAbility { get; private set; } = new();
    public Dictionary<string, Ability> DictActiveAbility { get; private set; } = new();
    public Dictionary<string, Ability> DictPassiveAbility { get; private set; } = new();

    public event Action OnListChanged;

    void Start()
    {
        if (m_ObjectAbility == null)
        {
            Debug.LogError("m_ObjectAbility is not assigned in LearnAbilityObject!");
            return;
        }

        Abilities.OnListChanged += UpdateDictionaries;
        UpdateDictionaries();
    }

    void OnDisable()
    {
        Abilities.OnListChanged -= UpdateDictionaries;
    }

    // public void LearnAbility(string name = "normal")
    // {
    //     LearnAbility(m_CommandDataFlyweight);
    // }

    public void LearnAbility(CommandDataFlyweight commandDataFlyweight)
    {
        if (m_AbilityManager == null)
        {
            Debug.LogWarning("GameManager or AbilityManager is null.");
            return;
        }

        if (m_AbilityManager.DictAbilityCollection.ContainsKey(commandDataFlyweight.CmdName))
        {
            ICommandUndoable command = new LearnAbilityCommand(this, m_ObjectT.ObjectId, commandDataFlyweight.CmdName, commandDataFlyweight.IdName);
            CommandInvoker.ExecuteCommand(command);
        }
        else
        {
            Debug.LogWarning($"Ability '{commandDataFlyweight.CmdName}' not available to be learned.");
        }
    }

    // public void UndoLearnAbility(string name = "normal")
    // {
    //     UndoLearnAbility(m_CommandDataFlyweight);
    // }

    public void UndoLearnAbility(CommandDataFlyweight commandDataFlyweight)
    {
        if (Abilities.Find(x => x.AbilityName == commandDataFlyweight.CmdName))
        {
            //undo the equip ability
            m_EquipAbilityObject.UndoEquipAbility(commandDataFlyweight.CmdName);

            CommandInvoker.UndoCommand(m_ObjectT.ObjectId.ToString(), commandDataFlyweight.IdName);
        }
        else
        {
            Debug.LogWarning($"Ability '{commandDataFlyweight.CmdName}' not available to be undo.");
        }
    }

    public void RedoLearnAbility(string name = "normal")
    {
        RedoLearnAbility(m_CommandDataFlyweight);
    }

    public void RedoLearnAbility(CommandDataFlyweight commandDataFlyweight)
    {
        // if (Abilities.Find(x => x.AbilityName == name))
        // {
        CommandInvoker.RedoCommand(m_ObjectT.ObjectId.ToString(), commandDataFlyweight.IdName);
        // }
        // else
        // {
        //     Debug.LogWarning($"Ability '{name}' not available to be redo.");
        // }
    }

    #region ICommand Methods

    public void ExecuteUndoable(string name = "normal")
    {
        if (m_AbilityManager == null)
        {
            Debug.LogWarning("GameManager or AbilityManager is null.");
            return;
        }

        if (m_AbilityManager.DictAbilityCollection.TryGetValue(name, out Ability ability))
        {
            Debug.Log($"Learning Ability: {name}");

            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectAbility,
                m_ObjectAbility.StatsData.DataNumVars,
                "Ability",
                dataNumVar => dataNumVar.NumVariable++
            );

            AddAbility(ability);
        }
    }

    public void Undo(string name = "normal")
    {
        if (Abilities.TryGetValue(x => x.AbilityName == name, out Ability ability))
        {
            Debug.Log($"Undoing learned Ability: {name}");

            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectAbility,
                m_ObjectAbility.StatsData.DataNumVars,
                "Ability",
                dataNumVar => dataNumVar.NumVariable--
            );

            if (Abilities.ContainsKey(ability))
            {
                Abilities.Remove(ability);
            }
        }
        else
        {
            Debug.LogWarning($"Cannot undo, ability '{name}' not found.");
        }
    }

    public void Redo(string name = "normal")
    {
        if (Abilities.TryGetValue(x => x.AbilityName == name, out Ability ability))
        {
            Debug.Log($"Redoing learned Ability: {name}");

            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectAbility,
                m_ObjectAbility.StatsData.DataNumVars,
                "Ability",
                dataNumVar => dataNumVar.NumVariable++
            );

            AddAbility(ability);
        }
        else
        {
            Debug.LogWarning($"Cannot redo, ability '{name}' not found.");
        }
    }

    #endregion

    private void AddAbility(Ability ability)
    {
        if (!Abilities.ContainsKey(ability))
        {
            Abilities.Add(ability);
        }
    }

    private void UpdateDictionaries()
    {
        //DictAbility.Clear();
        DictActiveAbility.Clear();
        DictPassiveAbility.Clear();

        foreach (var item in Abilities.Items)
        {
            //DictAbility[item.AbilityName] = item;

            if (item.IsPassiveAbility)  // Fixed typo from `IsPassvieAbility`
                DictPassiveAbility[item.AbilityName] = item;
            else
                DictActiveAbility[item.AbilityName] = item;
        }

        OnListChanged?.Invoke();
    }

    public void UpdateDictLog()
    {
        CommandInvoker.LogDictionaryContents(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp", "debug.log"));
    }
}
