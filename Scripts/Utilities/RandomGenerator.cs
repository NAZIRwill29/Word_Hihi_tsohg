using UnityEngine;

public static class RandomGenerator
{
    public static int GenerateRandomNumber(int min, int max)
    {
        int num = Random.Range(min, max);
        return num;
    }
}
