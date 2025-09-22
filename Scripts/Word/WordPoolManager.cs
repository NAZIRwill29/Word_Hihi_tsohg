using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordPoolManager", menuName = "WordSystem/WordPoolManager")]
public class WordPoolManager : ScriptableObject
{
    public List<WordCollectionCategory> WordCollections;
    private Dictionary<string, List<string>> wordCache = new Dictionary<string, List<string>>();

    public void Initialize()
    {
        foreach (var item in WordCollections)
        {
            item.LoadWordsFromTextAsset();
        }
        BuildWordCache();
    }

    private void BuildWordCache()
    {
        wordCache.Clear();

        foreach (var collection in WordCollections)
        {
            if (wordCache.ContainsKey(collection.CategoryNameData.Name))
            {
                Debug.LogWarning($"Duplicate category found: {collection.CategoryNameData.Name}. Ignoring duplicate.");
                continue;
            }

            wordCache.Add(collection.CategoryNameData.Name, collection.Words);
        }

        Debug.Log($"Word cache built successfully with {wordCache.Count} categories.");
    }

    /// <summary>
    /// Public method to manually refresh the word cache at runtime.
    /// Call this if WordCollections list changes dynamically.
    /// </summary>
    public void RefreshCache()
    {
        BuildWordCache();
    }

    public List<string> GetRandomWords(List<string> categories, int count)
    {
        List<string> combinedPool = new List<string>();

        // Combine all words from the requested categories
        foreach (var category in categories)
        {
            if (!wordCache.ContainsKey(category))
            {
                Debug.LogWarning($"Category '{category}' not found in word cache.");
                continue;
            }

            List<string> categoryWords = wordCache[category];
            if (categoryWords.Count == 0)
            {
                Debug.LogWarning($"Word pool for category '{category}' is empty.");
                continue;
            }

            combinedPool.AddRange(categoryWords);
        }

        if (combinedPool.Count == 0)
        {
            Debug.LogWarning($"No words available in the selected categories.");
            return new List<string>();
        }

        // Shuffle the combined pool
        ShuffleList(combinedPool);

        // Take up to 'count' unique words
        int finalCount = Mathf.Min(count, combinedPool.Count);
        return combinedPool.GetRange(0, finalCount);
    }

    public List<string> GetRandomWords(string category, int count)
    {
        if (!wordCache.ContainsKey(category))
        {
            Debug.LogWarning($"Category '{category}' not found in word cache.");
            return new List<string>();
        }

        List<string> sourcePool = new List<string>(wordCache[category]);

        if (sourcePool.Count == 0)
        {
            Debug.LogWarning($"Word pool for category '{category}' is empty.");
            return new List<string>();
        }

        // Shuffle the source pool
        ShuffleList(sourcePool);

        // Take up to 'count' unique words
        int finalCount = Mathf.Min(count, sourcePool.Count);
        return sourcePool.GetRange(0, finalCount);
    }

    /// <summary>
    /// Fisher-Yates shuffle algorithm
    /// reliable, fast, no duplicates.
    /// If you have 5 words and ask for 3 → you get 3 unique random ones.
    /// If you ask for 10 → you get all 5, randomly shuffled.
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
