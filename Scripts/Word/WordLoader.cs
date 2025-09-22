using System.Collections.Generic;
using UnityEngine;

public static class WordLoader
{
    public static void LoadWordsFromTextAsset(List<string> words, TextAsset wordTextAsset)
    {
        if (wordTextAsset != null)
        {
            string[] lines = wordTextAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            words.Clear();
            words.AddRange(lines);
        }
        else
        {
            // Debug.LogError("WordTextAsset not assigned.");
        }
    }
}
