using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadingMultipleStrings : MonoBehaviour
{
    // A LocalizedStringReference provides a simple interface to retrieving translated strings and their tables.
    public string stringTableCollectionName = "My Strings";

    // We will cache our translated strings
    public Text Text;

    void OnEnable()
    {
        // During initialization we start a request for the string and subscribe to any locale change events so that we can update the strings in the future.
        StartCoroutine(LoadStrings());
        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
    }

    void OnSelectedLocaleChanged(Locale obj)
    {
        StartCoroutine(LoadStrings());
    }

    IEnumerator LoadStrings()
    {
        // A string table may not be immediately available such as during initialization of the localization system or when a table has not been loaded yet.
        var loadingOperation = LocalizationSettings.StringDatabase.GetTableAsync(stringTableCollectionName);
        yield return loadingOperation;

        if (loadingOperation.Status == AsyncOperationStatus.Succeeded)
        {
            var stringTable = loadingOperation.Result;
            Text.text = GetLocalizedString(stringTable, "This is a test");
        }
        else
        {
            Debug.LogError("Could not load String Table\n" + loadingOperation.OperationException.ToString());
        }
    }

    string GetLocalizedString(StringTable table, string entryName)
    {
        // Get the table entry. The entry contains the localized string and Metadata
        var entry = table.GetEntry(entryName);
        return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here
    }
}
