using UnityEngine;

public static class WordEffectUtils
{
    public static string ReverseWord(string input)
    {
        char[] charArray = input.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }

    public static string ScrambleString(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= 1)
            return input;

        char[] chars = input.ToCharArray();
        System.Random rng = new System.Random();

        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);

            // Swap characters at positions i and j
            char temp = chars[i];
            chars[i] = chars[j];
            chars[j] = temp;
        }

        return new string(chars);
    }
}
