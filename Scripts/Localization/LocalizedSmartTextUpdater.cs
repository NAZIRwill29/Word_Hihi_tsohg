using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.SmartFormat.Extensions;
using System.Collections.Generic;

/// <summary>
/* 🛠 How to Use It:
Setup your localized string in the table with Smart Format:

Table: UI Strings
Key: ScoreText
Value: Score: {score}
Create Persistent Variables:

Go to Assets → Create → Localization → varibale group asset.
Add a group, e.g., game.
Add a variable named score (type: IntVariable).

In localized setting, in smart format -> add varibale group asset in persistent variable source

In the Inspector:
Assign the LocalizedString (e.g., Table: UI Strings, Entry: ScoreText)
Assign a TextMeshProUGUI component.

Change the Variable at Runtime:
// From anywhere in code:
LocalizedSmartTextUpdater.SetIntVariable("game", "score", 42);
This will update the localized text automatically if the variable is used in the Smart Format.
*/
/// </summary>

public class LocalizedSmartTextUpdater : MonoBehaviour
{
    [Header("Localization")]
    [Tooltip("Localized String with Smart Format that references PersistentVariables (e.g., {player-name}, {score})")]
    [SerializeField] private LocalizedString localizedString;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textComponent;

    void OnEnable()
    {
        localizedString.StringChanged += OnStringChanged;
        localizedString.RefreshString(); // Trigger initial update
    }

    void OnDisable()
    {
        localizedString.StringChanged -= OnStringChanged;
    }

    void OnStringChanged(string updatedText)
    {
        if (textComponent != null)
        {
            textComponent.text = updatedText;
        }
    }

    // Optional: use this to get/set persistent variables programmatically
    public static T GetPersistentVariable<T>(string group, string key) where T : class, IVariable
    {
        var source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();

        if (source != null && source.TryGetValue(group, out var rawGroup))
        {
            if (rawGroup is IDictionary<string, IVariable> groupDict &&
                groupDict.TryGetValue(key, out var variable))
            {
                return variable as T;
            }
        }

        Debug.LogWarning($"Variable '{key}' in group '{group}' not found or invalid.");
        return null;
    }

    // Example usage
    public static void SetStringVariable(string group, string key, string value)
    {
        var variable = GetPersistentVariable<StringVariable>(group, key);
        if (variable != null)
            variable.Value = value;
        else
            Debug.LogWarning($"Variable '{key}' not found or invalid.");
    }

    public static void SetStringVariable(string value)
    {
        SetStringVariable("MyStringsGroup", "word", value);
    }

    public static void SetIntVariable(string group, string key, int value)
    {
        var variable = GetPersistentVariable<IntVariable>(group, key);
        if (variable != null)
            variable.Value = value;
    }
}
