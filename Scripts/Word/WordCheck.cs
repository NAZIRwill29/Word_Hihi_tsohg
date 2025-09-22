using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class TextAssetGroup
{
    public NameDataFlyweight NameDataFlyweight;
    public TextAsset TextAsset;
}

[CreateAssetMenu(fileName = "WordCheck", menuName = "WordSystem/WordCheck", order = 1)]
public class WordCheck : ScriptableObject
{
    // 0             1          2          3          4       5      
    //english     melayu    indonesia   spanish     french  portuguese
    [SerializeField] private ListWrapper<TextAssetGroup> m_WordLists = new();
    private TextAsset m_WordList;

    public HashSet<string> WordsInHash { get; private set; } = new HashSet<string>();
    public List<string> WordsInList { get; private set; } = new List<string>();
    //success word created
    [SerializeField] private List<string> m_Successword = new List<string>();
    [SerializeField] private List<string> m_SuccessExorcismWord = new List<string>();
    //public bool IsHasSetupWordList { get; private set; }

    public IReadOnlyList<string> SuccessWords => m_Successword;  // Read-only access to success words
    private static readonly char[] WordSeparators = { ',', ' ', '\n', '\r', '/', '(', ')' };
    private List<string> wordsBuffer = new List<string>();
    [SerializeField] private int m_MaxSuccessWords = 20;
    [SerializeField] private List<TextAssetGroup> m_LetterSetsLists = new();
    public List<string> LetterSets { get; private set; } = new List<string>();

    /// <summary>
    /// Sets up the word list from the first available language (index 0).
    /// </summary>
    public void SetupWordList(string name = "English")
    {
        Debug.Log("SetupWordList 1");

        if (m_WordLists == null || m_WordLists.Items.Count == 0)
        {
            Debug.LogWarning("Word list array is empty. Setup aborted.");
            return;
        }

        Debug.Log("SetupWordList");

        if (m_WordLists.TryGetValue(x => x.NameDataFlyweight.Name == name, out TextAssetGroup textAssetGroup))
        {
            m_WordList = textAssetGroup.TextAsset;
        }
        else
        {
            Debug.LogWarning("Selected word list is null. Setup aborted.");
            return;
        }

        ConvertWordListToWordString();

        m_Successword.Clear();
    }

    /// <summary>
    /// Converts the word list from a text file into a list of words.
    /// Normalize: Trim and Uppercase all words.
    /// </summary>
    private void ConvertWordListToWordString()
    {
        if (m_WordList == null || string.IsNullOrWhiteSpace(m_WordList.text))
        {
            Debug.LogWarning("Word list text is empty or null.");
            return;
        }

        // Clear buffer instead of reallocating
        wordsBuffer.Clear();

        // Use Split, but store the result in a static buffer
        string[] wordArray = m_WordList.text.Split(WordSeparators, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in wordArray)
        {
            // Normalize each word: Trim + Uppercase
            string cleanWord = word.Trim().ToUpper();
            if (!string.IsNullOrEmpty(cleanWord))
                wordsBuffer.Add(cleanWord);
        }

        // Assign to Words as HashSet
        WordsInHash = new HashSet<string>(wordsBuffer);

        // Create a **deep copy** for WordsInList to avoid shared reference
        WordsInList = new List<string>(wordsBuffer);

        //IsHasSetupWordList = true;
    }

    public void SetupLetterSets(string name = "English")
    {
        LetterSets = GetLetterSets(m_LetterSetsLists.Find(x => x.NameDataFlyweight.Name == name));
    }

    /// <summary>
    /// Checks if a word exists in the word list.
    /// Normalize: Trim and Uppercase input word before checking.
    /// </summary>
    public bool CheckWordExist(string word)
    {
        if (WordsInHash == null || WordsInHash.Count == 0 || string.IsNullOrWhiteSpace(word))
        {
            Debug.LogWarning("Words / word is empty or null.");
            return false;
        }

        string normalizedWord = word.Trim().ToUpper();
        Debug.Log("normalizedWord = " + normalizedWord);
        return WordsInHash.Contains(normalizedWord);
    }

    public bool CheckSuccessWordDuplicate(string word, int maxDuplicateNum)
    {
        if (m_Successword == null || m_Successword.Count == 0 || string.IsNullOrWhiteSpace(word))
        {
            Debug.LogWarning("Words / word is empty or null.");
            return false;
        }

        string normalizedWord = word.Trim().ToUpper();
        Debug.Log("normalizedWord = " + normalizedWord);

        // Count occurrences of the normalized word
        int count = 0;
        foreach (var w in m_Successword)
        {
            if (w.ToUpper() == normalizedWord)
            {
                count++;
                if (count > maxDuplicateNum)
                    return false;
            }
        }

        return count > 0;
    }

    /// <summary>
    /// Registers a successfully found word. - delete first word if exceed max
    /// Normalize: Trim and Uppercase input word before adding.
    /// </summary>
    public void RegisterSuccessWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return;

        string normalizedWord = word.Trim().ToUpper();
        m_Successword.Add(normalizedWord);
        if (m_Successword.Count > m_MaxSuccessWords)
            m_Successword.RemoveAt(0);
    }

    public void RegisterSuccessExorcismWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return;

        string normalizedWord = word.Trim().ToUpper();
        if (!m_SuccessExorcismWord.Contains(normalizedWord))
        {
            m_SuccessExorcismWord.Add(normalizedWord);
        }
    }

    /// <summary>
    /// Clears all successfully found words.
    /// </summary>
    public void ClearSuccessWord()
    {
        m_Successword.Clear();
        m_SuccessExorcismWord.Clear();
    }

    /// <summary>
    /// Removes a specific word from the success list.
    /// Normalize: Trim and Uppercase input word before removing.
    /// </summary>
    public void RemoveSuccessWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return;

        string normalizedWord = word.Trim().ToUpper();
        m_Successword.Remove(normalizedWord);
    }

    public bool CheckIsLongestSuccessWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return false;

        if (m_Successword == null || m_Successword.Count == 0)
            return true;

        string longestWord = m_Successword[0];
        int maxLength = longestWord.Length;

        for (int i = 1; i < m_Successword.Count; i++)
        {
            int length = m_Successword[i].Length;
            if (length > maxLength)
            {
                longestWord = m_Successword[i];
                maxLength = length;
            }
        }
        string normalizedWord = word.Trim().ToUpper();
        return longestWord == normalizedWord;
    }

    public bool CheckIsShortestSuccessWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return false;

        if (m_Successword == null || m_Successword.Count == 0)
            return true;

        string shortestWord = m_Successword[0];
        int minLength = shortestWord.Length;

        for (int i = 1; i < m_Successword.Count; i++)
        {
            int length = m_Successword[i].Length;
            if (length < minLength)
            {
                shortestWord = m_Successword[i];
                minLength = length;
            }
        }
        string normalizedWord = word.Trim().ToUpper();
        return shortestWord == normalizedWord;
    }

    public string GetRandomWord()
    {
        if (WordsInList == null || WordsInList.Count == 0)
        {
            Debug.LogWarning("WordsInList is empty. Cannot get random word.");
            return string.Empty;
        }

        return WordsInList[Random.Range(0, WordsInList.Count)];
    }

    public string GetRandomWord(int minLength, int maxLength)
    {
        if (WordsInList == null || WordsInList.Count == 0)
        {
            Debug.LogWarning("WordsInList is empty. Cannot get random word.");
            return string.Empty;
        }

        // Filter the list to only include words within the length range
        List<string> filteredWords = WordsInList.FindAll(word => word.Length >= minLength && word.Length <= maxLength);

        if (filteredWords.Count == 0)
        {
            Debug.LogWarning($"No words found with length between {minLength} and {maxLength}.");
            return string.Empty;
        }

        // Return a random word from the filtered list
        return filteredWords[Random.Range(0, filteredWords.Count)];
    }

    public List<string> GetRandomWords(int minLength, int maxLength, int count)
    {
        if (count == 0) return new List<string>();
        if (WordsInList == null || WordsInList.Count == 0)
        {
            Debug.LogWarning("WordsInList is empty. Cannot get random words.");
            return new List<string>();
        }

        // Filter the list to only include words within the length range
        List<string> filteredWords = WordsInList.FindAll(word => word.Length >= minLength && word.Length <= maxLength);

        if (filteredWords.Count == 0)
        {
            Debug.LogWarning($"No words found with length between {minLength} and {maxLength}.");
            return new List<string>();
        }

        // Shuffle the filtered list to randomize order
        List<string> shuffledWords = new List<string>(filteredWords);
        for (int i = 0; i < shuffledWords.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledWords.Count);
            (shuffledWords[i], shuffledWords[randomIndex]) = (shuffledWords[randomIndex], shuffledWords[i]);
        }

        // Take the first 'count' unique words (or fewer if not enough available)
        int finalCount = Mathf.Min(count, shuffledWords.Count);
        List<string> result = shuffledWords.GetRange(0, finalCount);

        return result;
    }

    #region letter with word count
    //HEAVY
    public void SaveBottomLetterSetsToFile(string path, int bottomN = 10, int maxLetterCount = 5, int minWordCount = 1, string allowedLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
        var resultList = new List<ComboResult>();
        var combinations = new List<string>();

        GenerateLetterCombinations("", maxLetterCount, allowedLetters.ToUpper(), combinations);

        for (int i = 0; i < combinations.Count; i++)
        {
            string combo = combinations[i];
            int count = CountBuildableWords(combo);
            if (count >= minWordCount)
            {
                resultList.Add(new ComboResult { letters = combo, wordCount = count });
            }
        }

        // Manual sort by wordCount (ascending)
        for (int i = 0; i < resultList.Count - 1; i++)
        {
            for (int j = i + 1; j < resultList.Count; j++)
            {
                if (resultList[i].wordCount > resultList[j].wordCount)
                {
                    ComboResult temp = resultList[i];
                    resultList[i] = resultList[j];
                    resultList[j] = temp;
                }
            }
        }

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("Bottom " + bottomN + " Letter Sets with ≥" + minWordCount + " Words\n");

            int writtenCount = 0;
            for (int i = 0; i < resultList.Count && writtenCount < bottomN; i++)
            {
                writer.WriteLine("Letters: " + resultList[i].letters + " → Words: " + resultList[i].wordCount);
                writtenCount++;
            }
        }

        Debug.Log("Bottom " + bottomN + " letter sets saved to " + path);
    }

    private class ComboResult
    {
        public string letters;
        public int wordCount;
    }

    private void GenerateLetterCombinations(string prefix, int maxLength, string allowedLetters, List<string> output)
    {
        if (prefix.Length > 0)
        {
            output.Add(prefix);
        }

        if (prefix.Length == maxLength)
            return;

        for (int i = 0; i < allowedLetters.Length; i++)
        {
            GenerateLetterCombinations(prefix + allowedLetters[i], maxLength, allowedLetters, output);
        }
    }

    /// <summary>
    /// Returns how many words in the word list can be formed from the input letters.
    /// </summary>
    public int CountBuildableWords(string inputLetters)
    {
        if (string.IsNullOrWhiteSpace(inputLetters) || WordsInHash == null || WordsInHash.Count == 0)
            return 0;

        var letterCounts = GetLetterFrequency(inputLetters.ToUpper());
        int count = 0;

        foreach (var word in WordsInHash)
        {
            if (CanBuildFromLetters(word, letterCounts))
                count++;
        }

        return count;
    }

    /// <summary>
    /// Checks if any word in the list can be formed from the given letters.
    /// </summary>
    public bool CanBuildWord(string inputLetters)
    {
        if (string.IsNullOrWhiteSpace(inputLetters) || WordsInHash == null || WordsInHash.Count == 0)
            return false;

        var letterCounts = GetLetterFrequency(inputLetters.ToUpper());

        foreach (var word in WordsInHash)
        {
            if (CanBuildFromLetters(word, letterCounts))
                return true;
        }

        return false;
    }

    private Dictionary<char, int> GetLetterFrequency(string letters)
    {
        var freq = new Dictionary<char, int>();
        foreach (char c in letters)
        {
            if (char.IsLetter(c))
            {
                if (freq.ContainsKey(c))
                    freq[c]++;
                else
                    freq[c] = 1;
            }
        }
        return freq;
    }

    private bool CanBuildFromLetters(string word, Dictionary<char, int> availableLetters)
    {
        var wordFreq = GetLetterFrequency(word);
        foreach (var pair in wordFreq)
        {
            if (!availableLetters.TryGetValue(pair.Key, out int count) || count < pair.Value)
                return false;
        }
        return true;
    }

    private List<string> GetLetterSets(TextAssetGroup textAssetGroup)
    {
        var letterSets = new List<string>();

        if (textAssetGroup == null || textAssetGroup.TextAsset == null)
        {
            Debug.LogWarning("TextAssetGroup or its TextAsset is null.");
            return letterSets;
        }

        string[] lines = textAssetGroup.TextAsset.text.Split('\n');

        foreach (var line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            int dotIndex = trimmed.IndexOf('.');
            if (dotIndex >= 0 && dotIndex < trimmed.Length - 1)
            {
                string letters = trimmed.Substring(dotIndex + 1).Trim();
                if (!string.IsNullOrEmpty(letters))
                {
                    letterSets.Add(letters);
                }
            }
        }

        return letterSets;
    }

    public List<string> GetLetterSetsFromFile(string path)
    {
        var letterSets = new List<string>();

        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found: " + path);
            return letterSets;
        }

        string[] lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            int dotIndex = trimmed.IndexOf('.');
            if (dotIndex >= 0 && dotIndex < trimmed.Length - 1)
            {
                string letters = trimmed.Substring(dotIndex + 1).Trim();
                if (!string.IsNullOrEmpty(letters))
                {
                    letterSets.Add(letters);
                }
            }
        }

        return letterSets;
    }

    // public List<string> GetLetterSetsFromFileByWordCount(string path, int targetWordCount, MatchType matchType)
    // {
    //     var letterSets = new List<string>();
    //     var allLines = new List<string>();

    //     if (!File.Exists(path))
    //     {
    //         Debug.LogWarning("File not found: " + path);
    //         return letterSets;
    //     }

    //     allLines.AddRange(File.ReadAllLines(path));

    //     int closestDiff = int.MaxValue;
    //     List<Tuple<string, int>> candidates = new List<Tuple<string, int>>();

    //     for (int i = 0; i < allLines.Count; i++)
    //     {
    //         string line = allLines[i];

    //         if (line.StartsWith("Letters:"))
    //         {
    //             int arrowIndex = line.IndexOf("→");
    //             if (arrowIndex > 0)
    //             {
    //                 string letterPart = line.Substring(0, arrowIndex).Trim();  // "Letters: ABC"
    //                 string wordPart = line.Substring(arrowIndex + 1).Trim();    // "Words: 23"

    //                 int wordIndex = wordPart.IndexOf("Words:");
    //                 if (wordIndex >= 0)
    //                 {
    //                     string wordCountStr = wordPart.Substring(6).Trim();
    //                     int count;
    //                     if (int.TryParse(wordCountStr, out count))
    //                     {
    //                         string letters = letterPart.Replace("Letters:", "").Trim();

    //                         // Match logic
    //                         bool match = false;
    //                         int diff = Mathf.Abs(count - targetWordCount);

    //                         switch (matchType)
    //                         {
    //                             case MatchType.Exact:
    //                                 match = (count == targetWordCount);
    //                                 break;
    //                             case MatchType.GreaterOrEqual:
    //                                 match = (count >= targetWordCount);
    //                                 if (match && diff < closestDiff)
    //                                 {
    //                                     closestDiff = diff;
    //                                 }
    //                                 break;
    //                             case MatchType.LessOrEqual:
    //                                 match = (count <= targetWordCount);
    //                                 if (match && diff < closestDiff)
    //                                 {
    //                                     closestDiff = diff;
    //                                 }
    //                                 break;
    //                         }

    //                         if (match)
    //                         {
    //                             candidates.Add(new Tuple<string, int>(letters, count));
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     // Return best matches (closest to target)
    //     if (matchType == MatchType.GreaterOrEqual || matchType == MatchType.LessOrEqual)
    //     {
    //         foreach (var tuple in candidates)
    //         {
    //             if (Mathf.Abs(tuple.Item2 - targetWordCount) == closestDiff)
    //             {
    //                 letterSets.Add(tuple.Item1);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         foreach (var tuple in candidates)
    //         {
    //             letterSets.Add(tuple.Item1);
    //         }
    //     }

    //     return letterSets;
    // }

    // //HEAVY()
    // /// <summary>
    // /// Finds all letter combinations of given length that can build exactly targetWordCount words.
    // /// </summary>
    // public List<string> FindLetterSetsMatchingWordCount(int targetWordCount, int maxLetterCount = 5, string allowedLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    // {
    //     var results = new List<string>();

    //     // Generate all possible letter combinations (with repetition)
    //     var combinations = new List<string>();
    //     GenerateLetterCombinations("", maxLetterCount, allowedLetters.ToUpper(), combinations);

    //     foreach (var combo in combinations)
    //     {
    //         int count = CountBuildableWords(combo);
    //         if (count == targetWordCount)
    //         {
    //             results.Add(combo);
    //         }
    //     }

    //     return results;
    // }

    // //HEAVY()
    // private void GenerateLetterCombinations(string prefix, int remaining, string allowed, List<string> output)
    // {
    //     if (remaining == 0)
    //     {
    //         output.Add(prefix);
    //         return;
    //     }

    //     foreach (char c in allowed)
    //     {
    //         GenerateLetterCombinations(prefix + c, remaining - 1, allowed, output);
    //     }
    // }
    #endregion
}
