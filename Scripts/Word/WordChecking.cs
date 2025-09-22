using System.Collections.Generic;
using UnityEngine;

public static class WordChecking
{
    public static bool CheckIsWordExistInList(List<string> wordList, string newWord)
    {
        if (wordList == null || wordList.Count == 0 || string.IsNullOrWhiteSpace(newWord))
            return false;

        string normalizedWord = newWord.Trim().ToUpper();
        return wordList.Contains(normalizedWord);
    }

    public static bool CheckContainLetter(List<char> letters, string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return false;

        string normalizedWord = word.Trim().ToUpper();

        foreach (char letter in letters)
        {
            if (normalizedWord.Contains(char.ToUpper(letter)))
            {
                return true;
            }
        }
        return false;
    }

    public static bool TryGetFirstMatchingLetter(List<char> letters, string word, out char matchedLetter)
    {
        matchedLetter = default;

        if (string.IsNullOrWhiteSpace(word))
            return false;

        string normalizedWord = word.Trim().ToUpper();

        foreach (char letter in letters)
        {
            char upperLetter = char.ToUpper(letter);
            if (normalizedWord.Contains(upperLetter))
            {
                matchedLetter = letter; // Return the original casing from the letters list
                return true;
            }
        }

        return false;
    }

    public static List<string> GetCommonWords(List<string> listA, List<string> listB)
    {
        List<string> commonWords = new List<string>();

        if (listA == null || listB == null)
            return commonWords;

        // Normalize listB into a HashSet for faster lookup
        HashSet<string> normalizedListB = new HashSet<string>();
        foreach (string word in listB)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                string normalized = word.Trim().ToUpper();
                if (!normalizedListB.Contains(normalized))
                    normalizedListB.Add(normalized);
            }
        }

        // Compare each normalized word in listA
        foreach (string word in listA)
        {
            if (string.IsNullOrWhiteSpace(word))
                continue;

            string normalized = word.Trim().ToUpper();
            if (normalizedListB.Contains(normalized) && !commonWords.Contains(normalized))
            {
                commonWords.Add(normalized);
            }
        }

        return commonWords;
    }
}
